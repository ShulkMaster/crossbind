$libDir = ".\CrossBind.Parser\Lang\"
$pack = "CrossBind.Parser.Implementation"
$ourDir = ".\CrossBind.Parser\Implementation\"

if (Test-Path $ourDir) {
    Write-Host "Folder Exists"
}
else
{
    New-Item $ourDir -ItemType Directory
    Write-Host "Folder Created successfully"
}

Remove-Item "$ourDir*" -Recurse -Force

java -jar Gen\antlr.jar -lib $libDir -Dlanguage=CSharp -encoding UTF8 -Xexact-output-dir -package $pack -o $libDir .\CrossBind.Parser\Lang\HaibtLexer.g4

Move-Item .\CrossBind.Parser\Lang\HaibtLexer.cs -Destination $ourDir -Force

java -jar Gen\antlr.jar -lib $libDir -Dlanguage=CSharp -encoding UTF8 -Xexact-output-dir -package $pack -o $ourDir -visitor .\CrossBind.Parser\Lang\Haibt.g4