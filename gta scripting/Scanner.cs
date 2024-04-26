using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaScript
{
    public class Scanner
    {
        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();

        private int start = 0;
        private int current = 0;
        private int line = 1;

        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
        {
            {"callout", TokenType.CALLOUT },
            {"start", TokenType.START },
            {"when", TokenType.WHEN },
            {"false", TokenType.FALSE },
            {"true", TokenType.TRUE },
            {"null", TokenType.NULL }
        };

        

        public Scanner (string source)
        {
            this.source = source;
        }

        public List<Token> scanTokens()
        {
            while (!isAtEnd())
            {
                start = current;
                scanToken();
            }
            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '{': addToken(TokenType.LEFT_BRACE); break;
                case '}': addToken(TokenType.RIGHT_BRACE); break;
                case '(': addToken(TokenType.LEFT_PAREN); break;
                case ')': addToken(TokenType.RIGHT_PAREN); break;
                case ';': addToken(TokenType.SEMICOLON); break;
                case ',': addToken(TokenType.COMMA); break;
                case '"': String(); break;
                case '-': addToken(TokenType.MINUS); break;
                case '+': addToken(TokenType.PLUS); break;
                case '!': addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '/': addToken(TokenType.SLASH); break;
                case '*': addToken(TokenType.STAR); break; 
                default:
                    if (isDigit(c))
                    {
                        number();
                    }
                    if (isAlpha(c))
                    {
                        identifier();
                    }
                    break;
            }
        }
        private void number()
        {
            while (isDigit(peek())) advance();

            if(peek() == '.' && isDigit(peekNext()))
            {
                advance();
                while (isDigit(peek())) advance();
            }

            addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
        }

        private char peekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }
        private bool match(char expected)
        {
            if (isAtEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;
        }

        private void String()
        {
            while(peek() != '"' && !isAtEnd())
            {
                if (peek() == '\n') line++;
                advance();
            }

            advance();

            String value = source.Substring(start + 1, current - start - 2);
            addToken(TokenType.STRING, value);
        }
        private void identifier()
        {
            while (isAlphaNumeric(peek())) advance();

            string text = source.Substring(start, current - start);
            TokenType type;
            if(keywords.ContainsKey(text)) type = keywords[text];
            else type = TokenType.IDENTIFIER;
            addToken(type);
        }

        private Boolean isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
           (c >= 'A' && c <= 'Z') ||
            c == '_';
        }

        private Boolean isAlphaNumeric(char c)
        {
            return isAlpha(c) || isDigit(c);
        }

        private Boolean isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private Boolean isAtEnd()
        {
            return current >= source.Length;
        }

        private char peek()
        {
            return source[current];
        }

        private char advance()
        {
            return source[current++];
        }

        private void addToken(TokenType type)
        {
            addToken(type, null);
        }

        private void addToken(TokenType type, object literal)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }
    }
}
