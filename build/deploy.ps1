properties {
    Import-Module psake-contrib/teamcity.psm1

    ## Arguments
    # Project
    $artifact = Get-Value-Or-Default $artifact "null"

    # MsSql Database    
    $dbtype = Get-Value-Or-Default $dbtype "mssql"
    $dbhost = Get-Value-Or-Default $dbhost "localhost"
    $dbuser = Get-Value-Or-Default $dbuser "flatmate"
    $dbpassword = Get-Value-Or-Default $dbpassword "JbHgt6L1Z9smyQIgGexq"
    $dbname = "flatmate"

    # Directories
    $sqlScriptDir = Get-Value-Or-Default $sqlScriptDir "./_scripts"
    $sqlBackupDir = Get-Value-Or-Default $sqlBackupDir "/opt/apps/backup/flatmate/"
    $livePath = Get-Value-Or-Default $livePath "/opt/apps/live/"

    ## Deployment
    $appName = "flatmate"
    $mainProjectDir = "src/FlatMate.Web";
    
    $deployDir = $appName + "_new"
    $deployPath = Join-Path $livePath $deployDir

    $runPath = Join-Path $livePath $appName
   
    $pidFile = "flatmate.pid"
    $pidFilePath = Join-Path $runPath $pidFile

    # Change to root directory
    Set-Location "../"
}

FormatTaskName {
   ""
   ""
   Write-Host "Executing task: $taskName in $(Get-Location)" -foregroundcolor Cyan
}

# Alias

task Deploy -depends Unzip-App, Patch-AppSettings, Stop, Update-Database, Update-App, Start {
}

# Task

task Unzip-App {
    if ($artifact -eq "null") {
        throw "missing artifact"
    }

    # Create or clean deploy directory    
    if(!(Test-path $deployPath)) {
        Write-Host "Creating directory: $deployPath"
        exec { mkdir -m 774 $deployPath }
    }
    else {
        $removePath = Join-Path $deployPath "*"
        Write-Host "Clearing path: $removePath"
        Remove-Item $removePath -Recurse         
    }

    # Unzip artifact
    Write-Host "Unzip artifact: $artifact"
    $ProgressPreference = "SilentlyContinue"
    Expand-Archive $artifact $deployPath
}

task Patch-AppSettings -depends Unzip-App {
    $appSettings = Join-Path $deployPath "appsettings.Production.json"

    Write-Host "Patching appsettings: $appSettings"

    $settings = Get-Content $appSettings
    $settings = $settings -replace "##dbuser##", $dbuser -replace "##dbpassword##", $dbpassword
    $settings | Out-File $appSettings
}

task Stop {
    Write-Host "Checking for pidfile: $pidFilePath"

    # check for pid file
    if(!(Test-path $pidFilePath)) {
        Write-Host "Pidfile not found"
        return
    }

    # get process id
    $appId = Get-Content $pidFilePath

    # check for process
    Write-Host "Stopping process: $appId"
    Get-Process -Id $appId -ErrorAction SilentlyContinue | Stop-Process -PassThru

    # remove pid file
    Write-Host "Removing pidfile: $pidFilePath"
    Remove-Item $pidFilePath
}

task Update-Database -depends Patch-AppSettings, Stop {
    Write-Host "Type:     $dbtype"
    Write-Host "Server:   $dbhost"
    Write-Host "Database: $dbname"
    Write-Host "User:     $dbuser"
    Write-Host ""

    $cwd = Get-Location
    Set-Location $mainProjectDir
    Write-Host "Changed directory: $(Get-Location)"
        
    # exec { dotnet restore }
   
    # create backup
    exec { dotnet dbupdate backup --type $dbtype --host $dbhost --database $dbname --user $dbuser --password $dbpassword --scripts $sqlScriptDir --backup $sqlBackupDir }
    
    # executing scripts
    exec { dotnet dbupdate execute --type $dbtype --host $dbhost --database $dbname --user $dbuser --password $dbpassword --scripts $sqlScriptDir }

    Set-Location $cwd
}

task Update-App -depends Stop {
    Write-Host "Removing old deployment: $runPath"
    Remove-Item $runPath -Recurse

    Write-Host "Renaming folder: $deployPath to $runPath"
    Move-Item $deployPath $runPath
}

task Start {
    $cwd = Get-Location
    Set-Location $runPath
    Write-Host "Changed directory: $(Get-Location)" 

    # start app
    $app = Start-Process dotnet "FlatMate.Web.dll" -passthru

    # write pid file
    $app.Id | Out-File $pidFilePath
            
    Set-Location $cwd
}

function Get-Value-Or-Default($value, $default) {
    if (!$value -or $value -eq "") {
        return $default
    }

    return $value;
}