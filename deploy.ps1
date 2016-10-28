properties {
    Import-Module psake-contrib/teamcity.psm1

    # Project
    $mainProjectDir = "src/FlatMate.Web";
    $artifactDir = "/opt/apps/artifacts/flatmate"
    $liveDir = "/opt/apps/live/flatmate"
    $pidFile = "flatmate.pid"

    # MySql Database    
    $dbtype = Get-Value-Or-Default $dbtype "mysql"
    $dbhost = Get-Value-Or-Default $dbhost "localhost"
    $dbport = Get-Value-Or-Default $dbport 3306
    $dbuser = Get-Value-Or-Default $dbuser "root"
    $dbpassword = Get-Value-Or-Default $dbpassword "admin"
    $dbname = "flatmate"
}

FormatTaskName {
   ""
   ""
   Write-Host "Executing Task: $taskName" -foregroundcolor Cyan
}

# Alias

task Deploy -depends Update-App, Update-AppSettings, Start {
}

# Task

task Dotnet-DbUpdate {
    $cwd = Get-Location
    Set-Location $liveDir
   
    # create backup
    exec { dotnet dbupdate backup --type $dbtype --host $dbhost --port $dbport --database $dbname --user $dbuser --password $dbpassword --scripts "./_scripts" --backup "./_scripts/backup" }
    
    # executing scripts
    exec { dotnet dbupdate execute --type $dbtype --host $dbhost --port $dbport --database $dbname --user $dbuser --password $dbpassword --scripts "./_scripts" }

    Set-Location $cwd
}

task Stop {
    $cwd = Get-Location
    Set-Location $liveDir
    
    # check for pid file
    if(!(Test-path $pidFile)) {
        return
    }

    # get process id
    $appId = Get-Content $pidFile

    # check for process
    if (Get-Process -Id $appId) {
        Stop-Process $appId
    }

    # remove pid file
    Remove-Item $pidFile
    
    Set-Location $cwd
}

task Update-App -depends Stop {
    $cwd = Get-Location
    Set-Location $liveDir
    
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

    $settings = Get-Content "appsettings.production.json"
    $settings = $settings -replace "##dbuser##", $dbuser -replace "##dbpassword##", $dbpassword
    $settings | Out-File "appsettings.production.json"
    
    Set-Location $cwd
}

task Start {
    $cwd = Get-Location
    Set-Location $liveDir

    # start app
    $app = Start-Process dotnet "FlatMate.Web.dll" -passthru

    # write pid file
    $app.Id | Out-File $pidFile
            
    Set-Location $cwd
}rm fl

function Get-Value-Or-Default($value, $default) {
    if (!$value -or $value -eq "") {
        return $default
    }

    return $value;
}