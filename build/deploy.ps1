properties {
    Import-Module psake-contrib/teamcity.psm1

    # Project
    $mainProjectDir = "src/FlatMate.Web";
    $artifactDir = "/opt/apps/artifacts/flatmate"
    $liveDir = "/opt/apps/live/flatmate"
    $pidFile = "flatmate.pid"

    # MsSql Database    
    $dbtype = Get-Value-Or-Default $dbtype "mssql"
    $dbhost = Get-Value-Or-Default $dbhost "localhost"
    $dbuser = Get-Value-Or-Default $dbuser "teamcity"
    $dbpassword = Get-Value-Or-Default $dbpassword "teamcity"
    $dbname = "flatmate"

    # Change to root directory
    Set-Location "../"
}

FormatTaskName {
   ""
   ""
   Write-Host "Executing Task: $taskName" -foregroundcolor Cyan
}

# Alias

task Deploy -depends Update-App, Update-AppSettings, Update-Database, Start {
}

# Task

task Update-Database {
    Write-Host "Type: `t`t $dbtype"
    Write-Host "Server: `t $dbhost"
    Write-Host "Database: `t $dbname"
    Write-Host "User: `t`t $dbuser"
    Write-Host ""

    $cwd = Get-Location
    Set-Location $mainProjectDir
    Write-Host "Working directory: " (Get-Location)
        
    exec { dotnet restore }
   
    # create backup
    exec { dotnet dbupdate backup --type $dbtype --host $dbhost --database $dbname --user $dbuser --password $dbpassword --scripts "./_scripts" --backup "./_scripts/backup" }
    
    # executing scripts
    exec { dotnet dbupdate execute --type $dbtype --host $dbhost --database $dbname --user $dbuser --password $dbpassword --scripts "./_scripts" }

    Set-Location $cwd
}

task Stop {
    $cwd = Get-Location
    Set-Location $liveDir
    Write-Host "Working directory: " (Get-Location)
    
    # check for pid file
    if(!(Test-path $pidFile)) {
        Set-Location $cwd
        return
    }

    # get process id
    $appId = Get-Content $pidFile

    # check for process
    Get-Process -Id $appId -ErrorAction SilentlyContinue | Stop-Process -PassThru

    # remove pid file
    Remove-Item $pidFile
    
    Set-Location $cwd
}

task Update-App -depends Stop {
    $cwd = Get-Location
    Set-Location $liveDir
    Write-Host "Working directory: " (Get-Location)
    
    # remove items in directory
    Remove-Item "./*" -recurse
    
    # get latest zip
    $files = Get-ChildItem $artifactDir -Filter *.zip
    $file = $artifactDir + "/" + $files[-1]

    # unzip and disable progressbar
    Write-Host "Deploying $file"
    $ProgressPreference = "SilentlyContinue"
    Expand-Archive $file $liveDir
    
    Set-Location $cwd
}

task Update-AppSettings -depends Update-App {
    $cwd = Get-Location
    Set-Location $liveDir 
    Write-Host "Working directory: " (Get-Location)

    $settings = Get-Content "appsettings.Production.json"
    $settings = $settings -replace "##dbuser##", $dbuser -replace "##dbpassword##", $dbpassword
    $settings | Out-File "appsettings.Production.json"
    
    Set-Location $cwd
}

task Start {
    $cwd = Get-Location
    Set-Location $liveDir
    Write-Host "Working directory: " (Get-Location)

    # start app
    $app = Start-Process dotnet "FlatMate.Web.dll" -passthru

    # write pid file
    $app.Id | Out-File $pidFile
            
    Set-Location $cwd
}

function Get-Value-Or-Default($value, $default) {
    if (!$value -or $value -eq "") {
        return $default
    }

    return $value;
}