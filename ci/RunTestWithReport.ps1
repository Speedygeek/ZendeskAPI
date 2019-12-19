
$resultsDir = "$($PSScriptRoot)/../src/TestResults/Coverage/"
$resultsDir = [IO.Path]::GetFullPath($resultsDir)
$resultFile = Join-Path -Path $resultsDir -ChildPath "coverage.cobertura.xml"
$reportTargetDir = Join-Path -Path $resultsDir -ChildPath "html"

$testProject = [IO.Path]::GetFullPath("$($PSScriptRoot)/../src/Speedygeek.ZendeskAPI.UnitTests/Speedygeek.ZendeskAPI.UnitTests.csproj")

dotnet test $testProject -c Debug  /p:CollectCoverage=true /p:CoverletOutputFormat=Cobertura /p:CoverletOutput=$resultsDir /p:ExcludeByAttribute="Obsolete%2cGeneratedCodeAttribute%2cCompilerGeneratedAttribute"
reportgenerator -reports:$resultFile -targetdir:$reportTargetDir -reporttypes:"HtmlInline_AzurePipelines_Dark;"

Start-Process "$reportTargetDir/index.htm"