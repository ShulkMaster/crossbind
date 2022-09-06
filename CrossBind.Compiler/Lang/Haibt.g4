grammar Haibt;

fragment DIGIT : [0-9] ;
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment HEX  : [0-9A-Fa-f] ;

NUMBER: DIGIT+ ([.,_] DIGIT+)? ;
VALID_START: LOWERCASE | UPPERCASE | '_';
VALID_FOLLOW: VALID_START | DIGIT;

// Keywords
EVENT: 'event';
PROP: 'prop';
TRUE: 'true';
FALSE: 'false';
IMPORT: 'import';
TYPE: 'type';
CONST: 'const';
LET: 'let';
FROM: 'from';

// CSS Rule properties
BackgroundColor : 'backgroundColor' | 'background-color';
Border : 'border';
Padding: 'padding';
Margin: 'margin';
Color: 'color';
Height: 'height';
Width: 'width';
FontSize: 'fontSize' | 'font-size';
Cursor : 'cursor';
Display: 'display';
ZINDEX: 'zIndex' | 'z-index';
BORDER_RADIUS : 'borderRadius';

WhiteSpaces: [\t\u000B\u000C\u0020\u00A0]+ -> channel(HIDDEN);
LineTerminator: [\r\n\u2028\u2029] -> channel(HIDDEN);
DOT: '.';
SEMI: ';';
COLON: ':';
EQ: '=';
QUEST: '?';
COMPONENT: 'component';
EXTENDS: 'extends';
PLUS: '+';
MINUS: '-';
TIMES: '*';
DIV: '/';
DOUBLEQ: '"';
STRING: DOUBLEQ .*? DOUBLEQ;
Variant: 'variant';
HEX_COLOR: '#' (HEX HEX HEX HEX HEX HEX| HEX HEX HEX);
CSS_UNIT: 'px' | 'em' | 'rem';
SING: PLUS | MINUS;
NONE: 'none';
SHADES: '0' | '1' | '2' | '3' | '4' | '5';
BOOLEAN: TRUE | FALSE;
MATH_OPERATOR: PLUS | MINUS | TIMES | DIV;
BORDER_STYLE: 'dotted' | 'dashed' | 'solid' | 'double' | NONE | 'hidden';
ACTION_STYLE: 'disabled' | 'active' | 'hoover' | NONE;
PRIMIRIVE_TYPE: 'string' | 'number' | 'bool' | Color;
const_value: NUMBER | STRING | BOOLEAN; 

CANNON_COMP: 'button' | 'select' | 'textbox';
// must be last to avoid overlapping
IDENTIFIER: VALID_START + VALID_FOLLOW*;

translationUnit:
    importStatement*
    (css_rule | compDeclaration)*
    EOF
    ;
    
importStatement: FROM STRING IMPORT '{' IDENTIFIER (',' IDENTIFIER)* '}' SEMI;

css_rule: 
    complex_rule |
    simple_rule ;
    
simple_rule: DOT IDENTIFIER '{' css_statement* '}';

complex_rule: DOT IDENTIFIER (DOT IDENTIFIER)+ '{' css_statement* '}';

compDeclaration: 
    COMPONENT IDENTIFIER (EXTENDS CANNON_COMP)? '{' body '}';

body:
    (css_statement | variant | script | property)*
;

css_statement: 
      (BackgroundColor ':' color_stm SEMI) #bgColor
    |  Border ':' borderValue #inlineBorder
    | ( Border ':' '{' borderValue borderValue? borderValue? borderValue? '}') #compoundBorder
    | ZINDEX ':' NUMBER SEMI #zIndex
    | BORDER_RADIUS ':' cssMeasure SEMI #borderRadius
    | Margin ':' clockRule SEMI #margin
    | Padding ':' clockRule SEMI #padding
    | Width ':' cssMeasure SEMI #width
    | Height ':' cssMeasure SEMI #height
    ;

borderValue : cssMeasure? BORDER_STYLE color_stm? SEMI;

color_stm:
    HEX_COLOR ('['SING? SHADES ']')? # consColor |
    IDENTIFIER ('['SING? SHADES ']')? # refColor;  

clockRule : cssMeasure cssMeasure? cssMeasure? cssMeasure?;
cssMeasure : NUMBER CSS_UNIT? ;
    
    
variant : 
    Variant(QUEST)? IDENTIFIER (EQ IDENTIFIER)? SEMI #variantDeclaration
    | IDENTIFIER variant_style #variantInitialization
    | IDENTIFIER variant_action #variantAction
    ;
    
variant_style : IDENTIFIER '{' css_statement* '}';

variant_action : IDENTIFIER ACTION_STYLE '{' css_statement* '}';

script: declaration | assigment | initialization;

declaration: (CONST | LET) IDENTIFIER COLON (PRIMIRIVE_TYPE | IDENTIFIER) SEMI;

property:
 PROP IDENTIFIER (COLON type_val QUEST?)? EQ value SEMI # autoInit |
 PROP IDENTIFIER COLON type_val QUEST? SEMI # declared;
 
value: const_value | IDENTIFIER | exp;
type_val: PRIMIRIVE_TYPE | IDENTIFIER;

assigment:
    IDENTIFIER EQ IDENTIFIER SEMI # byref |
    IDENTIFIER EQ const_value SEMI # byconst |
    IDENTIFIER EQ exp SEMI # byexp;
    
initialization:
   initializer IDENTIFIER SEMI # initbyref |
   initializer const_value SEMI # initbyconst |
   initializer exp SEMI # initbyexp;

initializer: (CONST | LET) IDENTIFIER (COLON (PRIMIRIVE_TYPE | IDENTIFIER))? EQ;
    
exp: NUMBER MATH_OPERATOR NUMBER SEMI;