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
ZINDEX: 'zIndex';
BORDER_RADIUS : 'borderRadius';

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
CSS_UNIT: 'px' | 'em' | 'rem';
BORDER_STYLE: 'dotted' | 'dashed' | 'solid' | 'double' | 'none' | 'hidden';

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
      (BackgroundColor ':' HEX_COLOR SEMI) #bgColor
    |  Border ':' borderValue #inlineBorder
    | ( Border ':' '{' borderValue borderValue? borderValue? borderValue? '}') #compoundBorder
    | ZINDEX ':' NUMBER SEMI #zIndex
    | BORDER_RADIUS ':' cssMeasure SEMI #borderRadius
    | Margin ':' clockRule SEMI #margin
    | Padding ':' clockRule SEMI #padding
    | Width ':' cssMeasure SEMI #width
    | Height ':' cssMeasure SEMI #height
    ;

borderValue : cssMeasure HEX_COLOR SEMI;

clockRule : cssMeasure cssMeasure? cssMeasure? cssMeasure?;
cssMeasure : NUMBER CSS_UNIT? ;
    
    
variant : 
    Variant IDENTIFIER SEMI #varianDeclaration
    | Variant IDENTIFIER '{' variant_style* '}' #variantInitialization
    ;
    
variant_style : IDENTIFIER '{' css_statement* '}';
    
variant_ext_initialization :
    IDENTIFIER '.' variant_style;
