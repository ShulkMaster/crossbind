namespace CrossBind.Compiler.Test.Samples;

public static class ButtonSample
{
    /// <summary>
    /// The number of expected tokens for the SimpleValidButton sample
    /// with out the White Space
    /// </summary>
    public const int SimpleValidButtonTokensNoWs = 6;

    public const string SimpleValidButton = @"component ButtonApp extends button {}";

    public const string ValidButtonWithBgColor =
        @"
from './dimends' import { Padding, Vue };

component ButtonApp extends button {
    backgroundColor : #F0D;
    variant mode;
    variant kind;
    border: 1px solid #ddd;
}";
}