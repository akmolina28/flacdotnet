using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetadata.IO
{
    public enum CharType
    {
        //LowerCaseLetter, 
        //UpperCaseLetter, 
        Hyphen,
        Numbers,
        Bracket,
        Punctuation,
        Slash,
        Separator,
        Other,
        WhiteSpace,
        Letters,
        None
    }
    public class ComplexToken
    {
        public CharType[] Token { get; set; }
        public string[] SubStrings { get; set; }
        public string StringRepresentation { get; private set; }

        public ComplexToken() { }

        public ComplexToken(string token)
        {
            StringRepresentation = token;
            GenerateToken(token);
        }

        private void GenerateToken(string s)
        {
            List<CharType> token = new List<CharType>();
            List<string> substrings = new List<string>();

            foreach (char c in s)
            {
                CharType type = GetCharType(c);

                if (type == CharType.Letters)
                {
                    if (token.Count == 0 || token.Last() != CharType.Letters)
                    {
                        token.Add(type);
                        substrings.Add(c.ToString());
                    }
                    else
                    {
                        string last = substrings.Last();
                        last += c;
                        substrings.RemoveAt(substrings.Count - 1);
                        substrings.Add(last.ToString());
                    }
                }
                else if (type == CharType.Numbers)
                {
                    if (token.Count == 0 || token.Last() != CharType.Numbers)
                    {
                        token.Add(type);
                        substrings.Add(c.ToString());
                    }
                    else
                    {
                        string last = substrings.Last();
                        last += c;
                        substrings.RemoveAt(substrings.Count - 1);
                        substrings.Add(last.ToString());
                    }
                }
                else if (type == CharType.WhiteSpace)
                {
                    if (token.Count == 0 || token.Last() != CharType.WhiteSpace)
                    {
                        token.Add(type);
                        substrings.Add(c.ToString());
                    }
                    else
                    {
                        string last = substrings.Last();
                        last += c;
                        substrings.RemoveAt(substrings.Count - 1);
                        substrings.Add(last.ToString());
                    }
                }
                else if (type == CharType.Separator)
                {
                    if (token.Count == 0 || token.Last() != CharType.Separator)
                    {
                        token.Add(type);
                        substrings.Add(c.ToString());
                    }
                    else
                    {
                        string last = substrings.Last();
                        last += c;
                        substrings.RemoveAt(substrings.Count - 1);
                        substrings.Add(last.ToString());
                    }
                }
                else
                {
                    token.Add(type);
                    substrings.Add(c.ToString());
                }
            }
            Token = token.ToArray();
            SubStrings = substrings.ToArray();
        }

        public bool MatchesTokenFormat(string s)
        {
            ComplexToken ct = new ComplexToken(s);
            return (this.Equals(ct));
        }

        public bool StringContainsToken(string s)
        {
            ComplexToken tokenized = new ComplexToken(s);

            for (int i = 0; i < tokenized.Token.Length; i++)
            {
                for (int j = 0; j < Token.Length; j++)
                {
                    if (i + j == tokenized.Token.Length)
                    {
                        break;
                    }
                    if (tokenized.Token[i + j] == this.Token[j])
                    {
                        if (j == Token.Length - 1)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;
        }

        private CharType GetCharType(char c)
        {
            if (c == '-')
            {
                return CharType.Hyphen;
            }
            if (Char.IsLetter(c))
            {
                return CharType.Letters;
            }
            else if (Char.IsDigit(c))
            {
                return CharType.Numbers;
            }
            else if (Char.IsPunctuation(c))
            {
                return CharType.Punctuation;
            }
            else if (Char.IsWhiteSpace(c))
            {
                return CharType.WhiteSpace;
            }
            else if (Char.IsSeparator(c))
            {
                return CharType.Separator;
            }
            else
            {
                return CharType.Other;
            }
        }

        //public string ReplaceTokenMatchWith(

        public override bool Equals(object obj)
        {
            ComplexToken t = (ComplexToken)obj;
            bool ret = true;

            if (t.Token.Length == this.Token.Length)
            {
                for (int i = 0; i < t.Token.Length; i++)
                {
                    if (t.Token[i] != this.Token[i])
                    {
                        ret = false;
                    }
                }
            }
            else
            {
                ret = false;
            }
            return ret;
        }

        public int LastIndexOfTokenInString(string s)
        {
            int index = IndexOfTokenInString(s);
            int ret = -1;

            string split = s.Substring(index);
            ComplexToken ct = new ComplexToken(split);
            
            if (index >= 0)
            {
                for (int i = 0; i < this.Token.Length; i++)
                {
                    index += ct.SubStrings[i].Length;
                }
                ret = index - 1;
            }

            return ret;
        }

        public string ReplaceTokenWith(string s, string replace)
        {
            string ret = s;
            int indexStart = IndexOfTokenInString(s);
            if (indexStart >= 0)
            {
                int indexEnd = LastIndexOfTokenInString(s);
                ret = s.Substring(0, indexStart) + replace + s.Substring(indexEnd + 1);
            }
            return ret;
        }

        public int IndexOfTokenInString(string s)
        {
            ComplexToken tokenized = new ComplexToken(s);
            int ret = 0;

            for (int i = 0; i < tokenized.Token.Length; i++)
            {
                if (i + this.Token.Length == this.Token.Length - 1)
                {
                    break;
                }
                for (int j = 0; j < this.Token.Length; j++)
                {
                    if (tokenized.Token[i + j] != this.Token[j])
                    {
                        break;
                    }
                    if (j + 1 == this.Token.Length)
                    {
                        // match! return length
                        return ret;
                    }
                }
                ret += tokenized.SubStrings[i].Length;
            }

            return -1;
        }

        public bool StringStartsWithToken(string s)
        {
            if (this.StringRepresentation == "")
            {
                return true;
            }
            ComplexToken ct = new ComplexToken(s);
            if (ct.Token.Length < this.Token.Length)
            {
                return false;
            }
            for (int i = 0; i < Token.Length; i++)
            {
                if (ct.Token[i] != this.Token[i])
                {
                    break;
                }
                if (i + 1 == this.Token.Length)
                {
                    return true;
                }
            }
            return false;
        }

        public bool StringEndsWithToken(string s)
        {
            if (this.StringRepresentation == "")
            {
                return true;
            }
            ComplexToken ct = new ComplexToken(s);
            if (ct.Token.Length < this.Token.Length)
            {
                return false;
            }
            for (int i = 0; i < Token.Length; i++)
            {
                if (ct.Token[ct.Token.Length - 1 - i] != this.Token[this.Token.Length - 1 - i])
                {
                    break;
                }
                if (i == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
