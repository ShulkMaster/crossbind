lexer grammar HaibtLexer;

import Style;

OpenBracket: '[';
CloseBracket: ']';
OpenParen: '(';
CloseParen: ')';
OpenBrace: '{';
CloseBrace: '}';
SemiColon: ';';
Comma: ',';
Assign: '=';
QuestionMark: '?';
Colon: ':';
DOT: '.';
Ellipsis: '...';

// operators
PlusPlus:                       '++';
MinusMinus:                     '--';
Plus: '+';

Not: '!';
Multiply: '*';
Divide: '/';
Modulus: '%';
Power: '**';
LessThan: '<';
MoreThan: '>';
LessThanEquals:                 '<=';
GreaterThanEquals:              '>=';
Equals_:                        '==';
NotEquals:                      '!=';
IdentityEquals:                 '===';
IdentityNotEquals:              '!==';
And: '&&' | 'and';
Or: '||' | 'or';
PlusAssign:                     '+=';
MinusAssign:                    '-=';
ARROW: '->' | '=>';

// Literal values
NullLiteral:                    'null';

BooleanLiteral: 'true' | 'false';

DecimalLiteral:                 DecimalIntegerLiteral DOT [0-9] [0-9_]* ExponentPart?
              |                 DOT [0-9] [0-9_]* ExponentPart?
              |                 DecimalIntegerLiteral ExponentPart?
              ;
              
StringLiteral: '"' DoubleStringCharacter* '"';

BackTick:  '`' -> pushMode(TEMPLATE);

// Keywords
Event: 'event';
Prop: 'prop';
Import: 'import';
TypeWord: 'type';
Const: 'const';
Let: 'let';
From: 'from';
COMPONENT: 'component';
Variant: 'variant';


// STD types
PRIMIRIVE_TYPE: 'string' | 'number' | 'bool' | Color;
HEX_COLOR: '#' HexDigit HexDigit HexDigit (HexDigit HexDigit HexDigit)?;

// comments
HtmlComment: '<!--' .*? '-->' -> channel(HIDDEN);
JsxComment: '{/*' .*? '*/}' -> channel(HIDDEN);
MultiLineComment: '/*' .*? '*/' -> channel(HIDDEN);
SingleLineComment: '//' ~[\r\n\u2028\u2029]* -> channel(HIDDEN);
WhiteSpaces: [\t\u000B\u000C\u0020\u00A0]+ -> channel(HIDDEN);
LineTerminator: [\r\n\u2028\u2029] -> channel(HIDDEN);

mode TEMPLATE;

BackTickInside:                 '`' -> type(BackTick), popMode;
TemplateStringStartExpression:  '${' -> pushMode(DEFAULT_MODE);
TemplateStringAtom:             ~[`];

// html tags declarations
mode TAG;

TagOpen: LessThan -> pushMode(TAG);

TagClose: MoreThan -> popMode;

TagSlashClose: Divide MoreThan  -> popMode;

TagSlash: Divide;

TagName
    : TagNameStartChar TagNameChar*
    ;
    
AttributeValue
    : [ ]* Attribute -> popMode
    ;
    
Attribute
    : DoubleQuoteString
    | AttributeChar
    | HexChars
    | DecChars
    ;
    
mode ATTVALUE;

TagEquals
    : Assign -> pushMode(ATTVALUE)
    ;
    
fragment AttributeChar
    : '-'
    | '_'
    | '.'
    | '/'
    | '+'
    | ','
    | '?'
    | '='
    | ':'
    | ';'
    | '#'
    | [0-9a-zA-Z]
    ;
    
fragment HexChars: '#' [0-9a-fA-F]+;
fragment DecChars: [0-9]+ '%'?;

fragment DoubleQuoteString: '"' ~[<"]* '"';
  
fragment TagNameStartChar:
    [:a-zA-Z]
    |   '\u2070'..'\u218F'
    |   '\u2C00'..'\u2FEF'
    |   '\u3001'..'\uD7FF'
    |   '\uF900'..'\uFDCF'
    |   '\uFDF0'..'\uFFFD'
    ;
    
fragment TagNameChar:
    TagNameStartChar
    | '-'
    | '_'
    | '.'
    | Digit
    |   '\u00B7'
    |   '\u0300'..'\u036F'
    |   '\u203F'..'\u2040'
    ;

fragment DoubleStringCharacter
    : ~["\\]
    | '\\' EscapeSequence
    | LineContinuation
    ;
    
fragment EscapeSequence
    : CharacterEscapeSequence
    | '0' // no digit ahead! TODO
    | HexEscapeSequence
    ;

fragment CharacterEscapeSequence
    : SingleEscapeCharacter
    | NonEscapeCharacter
    ;

fragment HexEscapeSequence
    : 'x' HexDigit HexDigit
    ;

fragment SingleEscapeCharacter
    : ['"\\bfnrtv]
    ;

fragment NonEscapeCharacter
    : ~['"\\bfnrtv0-9xu\r\n]
    ;

fragment EscapeCharacter
    : SingleEscapeCharacter
    | [0-9]
    | [xu]
    ;

fragment LineContinuation
    : '\\' [\r\n\u2028\u2029]
    ;

fragment HexDigit: [_0-9a-fA-F];

fragment DecimalIntegerLiteral
    : '0'
    | [1-9] [0-9_]*
    ;

fragment ExponentPart
    : [eE] [+-]? [0-9_]+
    ;

