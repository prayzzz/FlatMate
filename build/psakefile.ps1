properties {
    Import-Module psake-contrib/teamcity.psm1

    $config = "Debug"

    $date = Get-Date -Format yyyy.MM.dd;
    $seconds = [math]::Round([datetime]::Now.TimeOfDay.TotalMinutes)
    $version = "$date.$seconds"

    $isTeamcity = $env:TEAMCITY_VERSION
    if ($isTeamCity) { TeamCity-SetBuildNumber $version }

    # Change to root directory
    Set-Location "../"
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

        Add-Type -assembly "system.io.compression.filesystem"
        [io.compression.zipfile]::CreateFromDirectory($Source, $destinationPath)
    }
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