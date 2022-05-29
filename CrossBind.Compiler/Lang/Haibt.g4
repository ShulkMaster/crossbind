grammar Haibt;

fragment DIGIT : [0-9] ;
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;

NUMBER: DIGIT+ ([.,_] DIGIT+)? ;
VALID_START: LOWERCASE | UPPERCASE | '_';
VALID_FOLLOW: VALID_START | DIGIT;

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
CANNON_COMP: 'button' | 'select' | 'textbox';
// must be last to avoid overlapping
IDENTIFIER: VALID_START + VALID_FOLLOW*;

translationUnit:
    importStatement*
    (libFile | compDeclaration)+
    EOF
    ;
    
importStatement: FROM STRING IMPORT '{' IDENTIFIER (',' IDENTIFIER)* '}' SEMI;

libFile: 
    ('lib' '{' '}')+
    ;

compDeclaration: 
    COMPONENT IDENTIFIER (EXTENDS CANNON_COMP)? '{' body '}';

body: .*?;