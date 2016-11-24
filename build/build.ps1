    properties {
    Import-Module psake-contrib/teamcity.psm1

    $date = Get-Date -Format yyyy.MM.dd;
    $minutes = [math]::Round([datetime]::Now.TimeOfDay.TotalMinutes)
    $version = "$date.$minutes"

    $config = Get-Value-Or-Default $env:CONFIGURATION "Release"
    $mainProjectDir = "src/FlatMate.Web";

    $branch = (git symbolic-ref --short -q HEAD) | Out-String
    $branch = $branch.Trim()

    # MsSql Database    
    $dbtype = Get-Value-Or-Default $dbtype "mssql"
    $dbhost = Get-Value-Or-Default $dbhost "localhost"
    # $dbport = Get-Value-Or-Default $dbport 3306
    $dbuser = Get-Value-Or-Default $dbuser "teamcity"
    $dbpassword = Get-Value-Or-Default $dbpassword "teamcity"
    $dbname = "ci_flatmate_$branch"

    # Teamcity
    $isTeamcity = $env:TEAMCITY_VERSION
    if ($isTeamCity) { TeamCity-SetBuildNumber $version }

    # Change to root directory
    Set-Location "../"

    Write-Host "Branch: $branch"
    Write-Host "Configuration: $config"
    Write-Host "Version: $version"
}

FormatTaskName {
   ""
   ""
   Write-Host "Executing Task: $taskName" -foregroundcolor Cyan
}

# Alias

task Restore -depends Yarn, Dotnet-Restore {
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

task Yarn {
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

task Compile-Typescript -depends Yarn {
    exec { src/FlatMate.Web/wwwroot/node_modules/.bin/tsc -p "$mainProjectDir/wwwroot/" }
}

task Compile-Sass -depends Yarn {
    exec { src/FlatMate.Web/wwwroot/node_modules/.bin/node-sass "$mainProjectDir/wwwroot/css/" --output "$mainProjectDir/wwwroot/css/" --output-style "compressed" }
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
    Set-Version "$mainProjectDir/project.json"
    Set-Version "$mainProjectDir/appsettings.json"
}

task Dotnet-Build -depends Dotnet-Restore, Set-Version {
    exec { dotnet build $mainProjectDir --configuration $config }
}

task Dotnet-Test -depends Dotnet-Build {
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

task Dotnet-DbUpdate -depends Dotnet-Restore  {
    Write-Host "Type: `t $dbtype"
    Write-Host "Server: `t $dbhost"
    Write-Host "Database: `t $dbname"
    Write-Host "User: `t $dbuser"
    Write-Host ""

    $cwd = Get-Location
    Set-Location $mainProjectDir

    # check for database
    Write-Host "Checking database $dbname"
    exec { sqlcmd -b -S $dbhost -U $dbuser -P $dbpassword -Q "USE [$dbname]" } | Out-Null
    
    # executing scripts
    exec { dotnet dbupdate execute --type $dbtype --host $dbhost --user $dbuser --password $dbpassword --database $dbname --scripts "./_scripts" }

    Set-Location $cwd
}

# Inserts the current version into the given json file.
function Set-Version ($file) {
    $project = ConvertFrom-Json -InputObject (Gc $file -Raw)
    $project.version = $version
    $project | ConvertTo-Json -depth 100 | Out-File $file
}

# Runs the tests in the given project
function Start-Test ($project) {
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

function Use-Object
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [AllowEmptyCollection()]
        [AllowNull()]
        [Object]
        $InputObject,
 
        [Parameter(Mandatory = $true)]
        [scriptblock]
        $ScriptBlock
    )
 
    try
    {
        . $ScriptBlock
    }
    finally
    {
        if ($null -ne $InputObject -and $InputObject -is [System.IDisposable])
        {
            $InputObject.Dispose()
        }
    }
}
