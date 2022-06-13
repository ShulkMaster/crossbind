grammar Haibt;

fragment DIGIT : [0-9] ;
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment HEX  : [0-9A-F] ;

NUMBER: DIGIT+ ([.,_] DIGIT+)? ;
VALID_START: LOWERCASE | UPPERCASE | '_';
VALID_FOLLOW: VALID_START | DIGIT;

// CSS Rule properties
BackgroundColor : 'backgroundColor';
Border : 'border';
Padding: 'padding';
Margin: 'margin';
Color: 'color';
Height: 'height';
Width: 'width';
FontSize: 'fontSize';
Cursor : 'cursor';
Display: 'display';

WhiteSpaces: [\t\u000B\u000C\u0020\u00A0]+ -> channel(HIDDEN);
LineTerminator: [\r\n\u2028\u2029] -> channel(HIDDEN);

SEMI: ';';
COLON: ':';
COMPONENT: 'component';
EXTENDS: 'extends';
IMPORT: 'import';
FROM: 'from';
SINGLEQ: '\'';
STRING: SINGLEQ .*? SINGLEQ;
Variant: 'variant';
HEX_COLOR: '#' (HEX HEX HEX HEX HEX HEX| HEX HEX HEX);

CANNON_COMP: 'button' | 'select' | 'textbox';
// must be last to avoid overlapping
IDENTIFIER: VALID_START + VALID_FOLLOW*;

translationUnit:
    importStatement*
    (libFile | compDeclaration)*
    EOF
    ;
    
importStatement: FROM STRING IMPORT '{' IDENTIFIER (',' IDENTIFIER)* '}' SEMI;

libFile: 
    ('lib' '{' '}')+
    ;

compDeclaration: 
    COMPONENT IDENTIFIER (EXTENDS CANNON_COMP)? '{' body '}';

body:
    (css_statement | variant | variant_ext_initialization)*
;

css_statement: 
      (BackgroundColor ':' HEX_COLOR SEMI)
    | ( Border ':' '{' .*? '}')
    ;
    
variant : 
    Variant IDENTIFIER SEMI #VarianDeclaration
    | Variant IDENTIFIER '{' variant_style* '}' #VariantInitialization
    ;
    
variant_style : IDENTIFIER '{' css_statement* '}';
    
variant_ext_initialization :
    IDENTIFIER '.' variant_style;