parser grammar Haibt;

options {
    tokenVocab=HaibtLexer;
}

const_value: StringLiteral | DecimalLiteral | BooleanLiteral; 

translationUnit:
    importStatement*
    (css_rule | compDeclaration)*
    EOF
    ;
    
importStatement: From StringLiteral Import '{' IDENTIFIER (',' IDENTIFIER)* '}' SemiColon;

css_rule: 
    complex_rule |
    simple_rule ;
    
simple_rule: DOT IDENTIFIER '{' css_statement* '}';

complex_rule: DOT IDENTIFIER (DOT IDENTIFIER)+ '{' css_statement* '}';

compDeclaration: 
    COMPONENT IDENTIFIER '{' body '}';

body:
    (css_statement | variant | script | property)* markup?
;

css_statement: 
      BackgroundColor ':' color_stm SemiColon #bgColor
    |  Border Colon borderValue #inlineBorder
    |  Border Colon OpenBrace borderValue borderValue? borderValue? borderValue? CloseBrace #compoundBorder
    | ZINDEX Colon DecimalLiteral SemiColon #zIndex
    | BORDER_RADIUS Colon cssMeasure SemiColon #borderRadius
    | Margin Colon clockRule SemiColon #margin
    | Padding Colon clockRule SemiColon #padding
    | Width Colon cssMeasure SemiColon #width
    | Height Colon cssMeasure SemiColon #height
    ;

borderValue : cssMeasure? BORDER_STYLE color_stm? SemiColon;

color_stm:
    HEX_COLOR ('['Sing? SHADES ']')? # consColor |
    IDENTIFIER ('['Sing? SHADES ']')? # refColor;  

clockRule : cssMeasure cssMeasure? cssMeasure? cssMeasure?;
cssMeasure : DecimalLiteral CSS_UNIT? ;
    
    
variant : 
    Variant(QuestionMark)? IDENTIFIER (Assign IDENTIFIER)? SemiColon #variantDeclaration
    | IDENTIFIER variant_style #variantInitialization
    | IDENTIFIER variant_action #variantAction
    ;
    
variant_style : IDENTIFIER OpenBrace css_statement* CloseBrace;

variant_action : IDENTIFIER ACTION_STYLE OpenBrace css_statement* CloseBrace;

script: declaration | assigment | initialization;

declaration: (Const | Let) IDENTIFIER Colon (PRIMIRIVE_TYPE | IDENTIFIER) SemiColon;

property:
 Prop IDENTIFIER (Colon type_val QuestionMark?)? Assign value SemiColon # autoInit |
 Prop IDENTIFIER Colon type_val QuestionMark? SemiColon # declared;
 
value: const_value | IDENTIFIER | singleExpression;
type_val: PRIMIRIVE_TYPE | IDENTIFIER;

assigment:
    IDENTIFIER Assign IDENTIFIER SemiColon # byref |
    IDENTIFIER Assign const_value SemiColon # byconst |
    IDENTIFIER Assign singleExpression SemiColon # byexp;
    
initialization:
   initializer IDENTIFIER SemiColon # initbyref |
   initializer const_value SemiColon # initbyconst |
   initializer singleExpression SemiColon # initbyexp;

initializer: (Const | Let) IDENTIFIER (Colon (PRIMIRIVE_TYPE | IDENTIFIER))? Assign;
    
expressionSequence
    : Ellipsis? singleExpression (',' Ellipsis? singleExpression)*
    ;
 
singleExpression
    : singleExpression PlusPlus # PostIncrementExpression
    | singleExpression MinusMinus # PostDecreaseExpression
    | '!' singleExpression # NotExpression
    | singleExpression ('*' | '/' | '%') singleExpression                   # MultiplicativeExpression
    | singleExpression ('+' | '-') singleExpression                         # AdditiveExpression
    | singleExpression ('==' | '!=' | '===' | '!==') singleExpression # EqualityExpression
    | singleExpression And singleExpression  # LogicalAndExpression
    | singleExpression Or singleExpression # LogicalOrExpression
    | literal # LiteralExpression
    | htmlElement # HtmlElementExpression
    | '(' expressionSequence ')' # ParenthesizedExpression
    ;

markup: htmlElement+;

htmlElement
    : '<' IDENTIFIER htmlAttribute* '>' htmlContent '<' '/' IDENTIFIER '>' #htmlChildren
    | '<' IDENTIFIER htmlAttribute* '/' '>' #htmlOptional
    | '<' IDENTIFIER htmlAttribute* '>' #htmlSingle
    ;
    
htmlContent
    : htmlChardata? ((htmlElement | objectExpressionSequence) htmlChardata?)*
    ;
    
htmlAttribute 
    : IDENTIFIER '=' htmlAttributeValue #attributeAssign
    | IDENTIFIER #attributeBoolean
    | '[' IDENTIFIER ']' #attributeBind;

htmlChardata
    : ~('<'|'{')+
    ;
    
htmlAttributeValue
    : AttributeValue
    | StringLiteral
    | objectExpressionSequence
    ;

objectExpressionSequence
    : '{' expressionSequence? '}'
    ;

literal
    : NullLiteral
    | BooleanLiteral
    | StringLiteral
    | numericLiteral
    ;

numericLiteral
    : DecimalLiteral
    ;

