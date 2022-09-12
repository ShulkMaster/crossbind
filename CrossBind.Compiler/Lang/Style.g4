lexer grammar Style;

//css units
CSS_UNIT: 'px' | 'em' | 'rem';
None: 'none';
Sing: '+' | '-';

SHADES: '0' .. '5';

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
BORDER_RADIUS : 'borderRadius' | 'border-radius';

BORDER_STYLE: 'dotted' | 'dashed' | 'solid' | 'double' | None | 'hidden';
ACTION_STYLE: 'disabled' | 'active' | 'hoover' | None;

Minus: '-';
IDENTIFIER: IdentifierStart VALID_FOLLOW*;
IdentifierStart:  [a-zA-Z] | [$_];
VALID_FOLLOW: IdentifierStart | Digit | Minus;

fragment Digit: [0-9];