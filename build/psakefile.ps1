properties {
    Import-Module psake-contrib/teamcity.psm1

    $config =  $env:CONFIGURATION
    if (!$config -or $config -eq "") {
        $config = "Debug"
    }

    $date = Get-Date -Format yyyy.MM.dd;
    $seconds = [math]::Round([datetime]::Now.TimeOfDay.TotalMinutes)
    $version = "$date.$seconds"

    # MySql Database    
    $dbhost = "localhost"
    $dbport = 3306    
    $dbuser = "root"
    $dbpassword = "admin"
    $dbname = "ci_$branch"
    $dbtype = "mysql"

    $isTeamcity = $env:TEAMCITY_VERSION
    if ($isTeamCity) { TeamCity-SetBuildNumber $version }

    # Change to root directory
    Set-Location "../"

    Write-Host "Configuration: $config"
    Write-Host "Version: $date"
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

# Tasks

task Npm-Install {
    exec {
        $cwd = Get-Location
        Set-Location "src/FlatMate.Web/wwwroot/"

        yarn

        Set-Location $cwd
    }
}

task Dotnet-Restore {
    exec { dotnet restore }
}

task Compile-Typescript -depends Npm-Install {
    exec { src/FlatMate.Web/wwwroot/node_modules/.bin/tsc -p "src/FlatMate.Web/wwwroot/" }
}

task Compile-Sass -depends Npm-Install {
    exec { src/FlatMate.Web/wwwroot/node_modules/.bin/node-sass "src/FlatMate.Web/wwwroot/css/" -o  "src/FlatMate.Web/wwwroot/css/" }
}

task Dotnet-Bundle -depends Dotnet-Restore, Compile-Typescript, Compile-Sass {
    exec {
        $cwd = Get-Location
        Set-Location "src/FlatMate.Web/"

        dotnet bundle

        Set-Location $cwd
    }
}

task Set-Version {
    Apply-Version("src/FlatMate.Web/project.json")
    Apply-Version("src/FlatMate.Web/appsettings.json")
}

task Dotnet-Build -depends Dotnet-Restore, Set-Version {
    exec { dotnet build "src/FlatMate.Web/" --configuration $config }
}

task Dotnet-Test -depends Dotnet-Build {
    #Run-Test("mobtima.Common.Test")
    #Run-Test("mobtima.Domain.Test")
    #Run-Test("mobtima.Persistence.Test")
    #Run-Test("mobtima.Web.Test")
}

task Dotnet-Publish -depends Dotnet-Bundle, Dotnet-Test {
    exec { dotnet publish "src/FlatMate.Web/" --configuration $config --no-build }
}

task Zip-Dotnet-Publish -depends Dotnet-Publish {
    exec {
        $source = "src/FlatMate.Web/bin/$config/netcoreapp1.0/publish"
        $destinationFolder = "dist/"
        $destinationFile = "flatmate-$version.zip"
        $destinationPath = $destinationFolder + $destinationFile

        If(!(Test-path $destinationFolder)) {
            mkdir $destinationFolder
        }

        If(Test-path $destinationPath) {
            Remove-item $destinationPath
        }

        Compress-Archive -Path $Source -DestinationPath $destinationPath
    }
}

task Dotnet-DbUpdate -depends Dotnet-Restore {
    $cwd = Get-Location
    Set-Location "src/FlatMate.Web/"

    # check for database
    mysql --user=$dbuser --password=$dbpassword -e "`"USE $dbname"`"
        
    # create database
    if ($lastexitcode -ne 0) {
        Write-Host "Creating database $dbname"
        exec { mysql --user=$dbuser --password=$dbpassword -e "`"CREATE DATABASE $dbname;"`" }

        exec { dotnet dbupdate init --type $dbtype --host $dbhost --port $dbport --database $dbname --user $dbuser --password $dbpassword --scripts "..\..\scripts" }
    }
    
    # executing scripts
    exec { dotnet dbupdate execute --type $dbtype --host $dbhost --port $dbport --database $dbname --user $dbuser --password $dbpassword --scripts "..\..\scripts" }

    Set-Location $cwd
}

function Apply-Version ($file) {
    $project = ConvertFrom-Json -InputObject (Gc $file -Raw)
    $project.version = $version
    $project | ConvertTo-Json -depth 100 | Out-File $file
}

function Run-Test ($project) {
    if ($isTeamCity) {
        TeamCity-TestSuiteStarted $project
    }

    exec { dotnet test "test/$project/" --configuration $config --result "TestResult.$project.xml" }

    if ($isTeamCity) {
        TeamCity-TestSuiteFinished $project
    }
}