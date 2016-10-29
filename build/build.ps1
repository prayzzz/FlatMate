properties {
    Import-Module psake-contrib/teamcity.psm1

    $date = Get-Date -Format yyyy.MM.dd;
    $seconds = [math]::Round([datetime]::Now.TimeOfDay.TotalMinutes)
    $version = "$date.$seconds"

    $config = Get-Value-Or-Default $env:CONFIGURATION "Debug"
    $mainProjectDir = "src/FlatMate.Web";

    # MySql Database    
    $dbtype = Get-Value-Or-Default $dbtype "mysql"
    $dbhost = Get-Value-Or-Default $dbhost "localhost"
    $dbport = Get-Value-Or-Default $dbport 3306
    $dbuser = Get-Value-Or-Default $dbuser "root"
    $dbpassword = Get-Value-Or-Default $dbpassword "admin"
    $dbname = "ci_flatmate_$branch"

    # Teamcity
    $isTeamcity = $env:TEAMCITY_VERSION
    if ($isTeamCity) { TeamCity-SetBuildNumber $version }

    # Change to root directory
    Set-Location "../"

    Write-Host "Configuration: $config"
    Write-Host "Version: $version"
}

FormatTaskName {
   ""
   ""
   Write-Host "Executing Task: $taskName" -foregroundcolor Cyan
}

# Alias

task Restore -depends Npm-Install, Dotnet-Restore {
}

task Bundle -depends Dotnet-Bundle {
}

task Build -depends Dotnet-Build {
}

task Test -depends Dotnet-Test {
}

task Publish -depends Zip-Dotnet-Publish {
}

task Update-Database -depends Dotnet-DbUpdate {
    # Invoke-psake .\build\psakefile.ps1 Update-Database -parameters @{"branch"="master"}
}

task CI-Build -depends Test, Update-Database {
}

# Tasks

task Npm-Install {
    exec {
        $cwd = Get-Location
        Set-Location "$mainProjectDir/wwwroot/"

        yarn

        Set-Location $cwd
    }
}

task Dotnet-Restore {
    exec { dotnet restore }
}

task Compile-Typescript -depends Npm-Install {
    exec { src/FlatMate.Web/wwwroot/node_modules/.bin/tsc -p "$mainProjectDir/wwwroot/" }
}

task Compile-Sass -depends Npm-Install {
    exec { src/FlatMate.Web/wwwroot/node_modules/.bin/node-sass "$mainProjectDir/wwwroot/css/" -o  "$mainProjectDir/wwwroot/css/" }
}

task Dotnet-Bundle -depends Dotnet-Restore, Compile-Typescript, Compile-Sass {
    exec {
        $cwd = Get-Location
        Set-Location $mainProjectDir

        dotnet bundle

        Set-Location $cwd
    }
}

task Set-Version {
    Insert-Version "$mainProjectDir/project.json"
    Insert-Version "$mainProjectDir/appsettings.json"
}

task Dotnet-Build -depends Dotnet-Restore, Set-Version {
    exec { dotnet build $mainProjectDir --configuration $config }
}

task Dotnet-Test -depends Dotnet-Build {
    #Run-Test("mobtima.Common.Test")
    #Run-Test("mobtima.Domain.Test")
    #Run-Test("mobtima.Persistence.Test")
    #Run-Test("mobtima.Web.Test")
}

task Dotnet-Publish -depends Dotnet-Bundle, Dotnet-Test {
    exec { dotnet publish $mainProjectDir --configuration $config --no-build }
}

task Zip-Dotnet-Publish -depends Dotnet-Publish {    
    $source = "$mainProjectDir/bin/$config/netcoreapp1.0/publish/*"
    $destinationFolder = "dist/"
    $destinationFile = "flatmate-$version.zip"
    $destinationPath = $destinationFolder + $destinationFile

    if(!(Test-path $destinationFolder)) {
        mkdir $destinationFolder
    }

    if(Test-path $destinationPath) {
        Remove-item $destinationPath
    }

    Compress-Archive -Path $Source -DestinationPath $destinationPath
}

task Dotnet-DbUpdate -depends Dotnet-Restore {
    $cwd = Get-Location
    Set-Location $mainProjectDir

    # check for database, no exec{} to continued execution and create missing db
    mysql --user=$dbuser --password=$dbpassword -e "`"USE $dbname"`"
        
    # create database
    if ($lastexitcode -ne 0) {
        Write-Host "Creating database $dbname"
        exec { mysql --user=$dbuser --password=$dbpassword -e "`"CREATE DATABASE $dbname;"`" }

        exec { dotnet dbupdate init --type $dbtype --host $dbhost --port $dbport --database $dbname --user $dbuser --password $dbpassword --scripts "./_scripts" }
    }
    
    # executing scripts
    exec { dotnet dbupdate execute --type $dbtype --host $dbhost --port $dbport --database $dbname --user $dbuser --password $dbpassword --scripts "./_scripts" }

    Set-Location $cwd
}

# Inserts the current version into the given json file.
function Insert-Version ($file) {
    $project = ConvertFrom-Json -InputObject (Gc $file -Raw)
    $project.version = $version
    $project | ConvertTo-Json -depth 100 | Out-File $file
}

# Runs the tests in the given project
function Run-Test ($project) {
    if ($isTeamCity) {
        TeamCity-TestSuiteStarted $project
    }

    exec { dotnet test "test/$project/" --configuration $config --result "TestResult.$project.xml" }

    if ($isTeamCity) {
        TeamCity-TestSuiteFinished $project
    }
}

function Get-Value-Or-Default($value, $default) {
    if (!$value -or $value -eq "") {
        return $default
    }

    return $value;
}