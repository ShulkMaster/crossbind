namespace CrossBind.Compiler.Test.Samples;

public static class UnitSample
{
    public const string BasicUnitSample =
        @"
from './dimends' import { Padding, Vue };

component ButtonApp extends button {
    backgroundColor : #F0D;
    variant mode;
    variant kind;
    border: 1 solid #F00;
    border: {
        1 solid #0F0;
    }
}
";
}