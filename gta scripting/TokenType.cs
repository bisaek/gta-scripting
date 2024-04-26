using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GtaScript
{
    public enum TokenType
    {
        CALLOUT,
        VAR,
        IDENTIFIER,
        START,
        LEFT_BRACE, RIGHT_BRACE,
        LEFT_PAREN, RIGHT_PAREN,
        SEMICOLON,
        EOF,
        STRING, NUMBER,
        COMMA,
        WHEN,
        BANG_EQUAL, EQUAL_EQUAL,
        GREATER, GREATER_EQUAL, LESS, LESS_EQUAL,
        BANG, EQUAL,
        MINUS, PLUS, SLASH, STAR,
        FALSE, TRUE, NULL

    }
}
