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
        STRING,
        COMMA,
        WHEN,

    }
}
