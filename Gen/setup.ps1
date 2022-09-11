$libDir = ".\CrossBind.Compiler\Lang\"
$pack = "CrossBind.Compiler.Parser"
$ourDir = ".\CrossBind.Compiler\Parser\"

java -jar Gen\antlr.jar -lib $libDir -Dlanguage=CSharp -encoding UTF8 -Xexact-output-dir -package $pack -o $libDir .\CrossBind.Compiler\Lang\HaibtLexer.g4

Move-Item .\CrossBind.Compiler\Lang\HaibtLexer.cs -Destination .\CrossBind.Compiler\Parser -Force

java -jar Gen\antlr.jar -lib $libDir -Dlanguage=CSharp -encoding UTF8 -Xexact-output-dir -package $pack -o $ourDir -visitor .\CrossBind.Compiler\Lang\Haibt.g4