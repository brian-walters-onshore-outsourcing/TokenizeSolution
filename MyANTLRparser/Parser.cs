using System;
using System.Collections.Generic;
using System.Linq;

namespace MyANTLRparser
{
    #region Types

    [Flags] public enum charType
    {
        whitespace                  = 1, // done
        letter                      = 2, // done
        digit                       = 4, // done
        underscore                  = 8, // done
        hexdigit                    = 16, // done
        binarydigit                 = 32, // done
        exponent                    = 64, // done
        minus                       = 128, //done
        symbol                      = 256, // done
        operatordigit               = 512, // done
        declarationsyntax           = 1024, // done
        special                     = 2048,  //done
 
        opening                     = 2*4096, // done
        closing                     = 4*4096, // done
        quote                       = 8*4096, // done
        letterordigit               =16*4096, // done
        validfirst4identifier       =32*4096, // done
        validsecond4identifier      =64*4096, // done
        letterordigitor_            =128*4096, // done
        punctuation                 =256*4096, // done
        realnumberpart              =1024*4096, // done
        integerpart                 =2048*4096, //done 
        endoffile                   =4096*4096, // done
        



    }

    public static class csharpclassifier 
    {
        static csharpclassifier()
        {
            Keywords = new HashSet<string>(new string[]
               {
                   "abstract", "as", "base", "bool", "break", "byte", "case",
                   "catch", "char", "checked", "class", "const","continue",
                   "decimal", "default", "delegate","do", "double", "else", "enum",
                   "event", "explicit", "extern", "false", "finally" , "fixed",
                   "float", "for", "foreach","goto", "if", "implicit", "in", "int",
                   "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly",
                   "ref","return","sbyte","sealed","short","sizeof","stackalloc",
                   "static","string","struct","switch","this","throw","true",
                  "try","typeof","uint","ulong","unchecked","unsafe","ushort",
                   "using","virtual","void","volatile","while"
               }
            );
            
            
        }
       public static charType IsWhatType(this char c)
        {
            charType rv = (charType)0;
            switch(c)
            {
                case ('_'): return charType.underscore|charType.validfirst4identifier|charType.validsecond4identifier|charType.letterordigitor_;
                case ('1'):
                case ('0'): return charType.digit | charType.binarydigit | charType.hexdigit | charType.validsecond4identifier | charType.letterordigitor_ | charType.realnumberpart | charType.integerpart;
                case 'A': case 'a': case 'B': case 'b': case 'C': case 'c':
                    return charType.hexdigit | charType.letter | charType.letterordigit |
                           charType.letterordigitor_ | charType.validfirst4identifier | charType.validsecond4identifier;
                case 'M': case 'm':
                    return charType.letter | charType.letterordigit |
                            charType.letterordigitor_ | charType.validfirst4identifier | charType.validsecond4identifier | charType.realnumberpart ;
                 case 'D': case 'd':case 'F': case 'f':
                    return charType.hexdigit | charType.letter | charType.letterordigit |
                            charType.letterordigitor_ | charType.validfirst4identifier | charType.validsecond4identifier | charType.realnumberpart;
                case 'E': case 'e': return charType.exponent |
                    charType.hexdigit | charType.letter | charType.letterordigit |
                    charType.letterordigitor_ | charType.validfirst4identifier |charType.validsecond4identifier | charType.realnumberpart;
                case '-': return charType.minus | charType.operatordigit | charType.symbol | charType.realnumberpart | charType.integerpart|charType.symbol;
                case '(': case '[': case '{': return charType.symbol | charType.opening|charType.declarationsyntax ;
                case '<': return charType.symbol | charType.opening | charType.declarationsyntax | charType.operatordigit;
                case ')': case ']': case '}': return charType.symbol | charType.closing | charType.declarationsyntax;
                case '>': return charType.symbol | charType.closing | charType.declarationsyntax | charType.operatordigit;
                case '"': case '\'': return charType.symbol | charType.quote ;
                 case ';': case '\\': case '@': case '#': case '$': case ',':
                    return charType.punctuation | charType.symbol | charType.special;
                case '.': return charType.punctuation | charType.symbol | charType.operatordigit | charType.realnumberpart;
                case 'U': case 'u': case 'L': case 'l':
                    return charType.letter | charType.letterordigit |
                        charType.letterordigitor_ | charType.integerpart;
                case ':': return charType.punctuation | charType.symbol | charType.operatordigit | charType.declarationsyntax;
                case '?':
                    return charType.symbol | charType.operatordigit | charType.declarationsyntax;
                case '+':
                case '!':
                case '%':
                case '^':
                case '&':
                case '*':
                case '=':
                case '|':
                case '/':
                case '~':
                    return charType.symbol | charType.operatordigit;
                case '\uffff':
                    return charType.endoffile;
             }
            if (char.IsWhiteSpace(c))  return charType.whitespace;
            if (char.IsDigit(c)) return charType.digit | charType.letterordigit | charType.letterordigitor_ | charType.validsecond4identifier ;
            if (char.IsLetter(c)) return charType.letter | charType.letterordigit | charType.letterordigitor_ | charType.validsecond4identifier | charType.validfirst4identifier;
            


            throw new Exception($"Unrecognized character '{c}' value: {(int)c}");
        }
       public static bool IsAnyOfTheseTypes(this char c, params charType[] types)
        {
            charType ct = c.IsWhatType();
            foreach (var type in types)
            {
                if (type == (ct & type)) return true;
            }
            return false;
        }
        public static bool IsAnyOfTheseTypes(this charType ct, params charType[] types)
        {
            
            foreach (var type in types)
            {
                if (type == (ct & type)) return true;
            }
            return false;
        }

        public static bool IsAnyOfTheseTypes(this tokenType ct, params tokenType[] types)
        {

            foreach (var type in types)
            {
                if (type == (ct & type)) return true;
            }
            return false;
        }

        public static bool IsAnyOfTheseTypes(this operatorType ct, params operatorType[] types)
        {

            foreach (var type in types)
            {
                if (type == (ct & type)) return true;
            }
            return false;
        }

        static  HashSet<string> Keywords ;
        public static bool IsKeyword(this string identifier)
        {
            return Keywords.Contains(identifier);
        }

    }

   
    public enum elementType
    {
       
        token,
        lineTerminator,
        whiteSpace,
        comment,
        preProcessorDirective
    }
    [Flags] public enum tokenType
    {
    
        literal                     = 1,
        literalString               = 2,
        literalNumber               = 4,
        name                        = 8,

        literalInteger              = 1 + 4 + 16,
        literalReal                 = 1 + 4 + 32,
        
        literalInterpolatedString   = 1 + 2 + 64,
        literalVerbatumString       = 1 + 2 + 128,
        literalChar                 = 1 + 256,
        

        identifier                  = 512,
        keyword                     = 1024,
 //       operatorPunctuator          = 2048,  // how is this different from below???
        @operator                   = 2048 + 4096,
        punctuator                  = 2048 + 2*4096,
        balanced                    = 2048 + 4*4096,

        literalIntegerHex           = 4096 + 1 + 4  + 16,
        literalIntegerBinary        = 2*4096 + 1 + 4 + 16,
        invalidToken                = 4*4096,

            


    }

    [Flags] public enum operatorType
    {
      
        primary                         =1,
        unary                           =2,
        range                           =3,

        arithmetic                      =4,
          multiplictive                 =4+8,
          additive                      =4+16,
          shift                         =4+32,
        conditional                     =64,
          relational                    =64 + 128,
          equality                      =64 + 256, 
        bitwise                         =512,
          bitwiseAND                    =512 + 1024,
          bitwiseXOR                    =512 + 2048,
          bitwiseOR                     =512 + 4096,
        logical                         = 2*4096,
          logicalAND                    = 2*4096 + 4*4096,
          logicalOR                     = 2*4096 + 8*4096,
          nullCoalescing                = 2*4096 + 16*4096,
        ternaryConditional              = 32*4096,
        assignment                      = 64*4096,
        lambdaDeclaration               = 128*4096,
        balanced                        = 256*4096,
        special                         = 512*4096,
        other                           = 1024*4096,



    }
    public class ParsedElement
    {
        public ParsedElement(string Data, elementType ElementType)
        {
            this.Data = Data;
            this.ElementType = ElementType;
        }
        public string Data { get; }
        public elementType ElementType { get; }

        #region tostring implementation
        protected virtual string MyName()
        {
            return "ParsedElement";
        }
        protected virtual string MyFirstRest()
        {
            return $"Type={ElementType,20}";
        }
        protected virtual string MyMiddleRest()
        {
            return $" Data='{Data}'";
        }
        protected virtual string MyLastRest()
        {
            return "";
        }
        public override string ToString()
        {
            return $"{MyName(),20}:{MyFirstRest()}{MyMiddleRest()}{MyLastRest()}";
        }
        #endregion tostring impl

    }

    public class ParsedToken : ParsedElement
    {
        public ParsedToken(string Data, tokenType TokenType)
            :base(Data,elementType.token)
        {
            this.TokenType = TokenType;
        }

        public tokenType TokenType { get; }

        #region tostring implementation
        protected override string MyName()
        {
            return "ParsedToken";
        }
        //protected override string MyFirstRest()
        //{
        //    return $"";
        //}
        //protected override string MyMiddleRest()
        //{
        //    return $"";
        //}
        protected override string MyLastRest()
        {
            return $"{base.MyLastRest()} TokenType:{TokenType}";
        }
        
        #endregion tostring impl
    }

    public class ParsedOperator : ParsedToken
    {
        public ParsedOperator(string Data, tokenType TokenType, operatorType OperatorType)
              :base(Data, TokenType)
            {
            this.OperatorType = OperatorType;
            }

        public operatorType OperatorType { get; }

        #region tostring implementation
        protected override string MyName()
        {
            return "ParsedOperator";
        }
        //protected override string MyFirstRest()
        //{
        //    return $"";
        //}
        //protected override string MyMiddleRest()
        //{
        //    return $"";
        //}
        protected override string MyLastRest()
        {
            return $"{base.MyLastRest()} OperatorType:{OperatorType}";
        }

        #endregion tostring impl
    }

    public class StateTransition
    {
 //       public char CharacterBeingParsed { get;  set; }
 //       public int PriorState { get;  set; }
        public string NewState { get; set; }
        public ParsedElement ElementFound { get; set; }

     
       



    }
    public class State
    {
        internal State() { }

        public string Name { get; internal set; }
        public Parser theParser { get; internal set; }
        public Func<State, char, StateTransition> Execute { get; internal set; }
    }

    #endregion needed types
    public class Parser
    {
        #region constructors

        public Parser (System.IO.StreamReader streamToParse)
        {
            streamBeingParsed = streamToParse;
        }
        
        public Parser (string stringToParse)
        {
            streamBeingParsed = new System.IO.StringReader(stringToParse);
        }

        #endregion

        #region private fields

        bool parsed;
        bool parsing;

        System.IO.TextReader streamBeingParsed;

        System.Text.StringBuilder collectedCharacters = new System.Text.StringBuilder(1000);
        System.Text.StringBuilder unconsumedCharacters = new System.Text.StringBuilder(100);

        List<ParsedElement> parsedElements = new List<ParsedElement>();

        List<ParsedToken> parsedTokens = new List<ParsedToken>();

        private Dictionary<string, State> StateMachine = new Dictionary<string, State>();
        private string currentState = "";

        bool pushed = false;
        char pushchar;
        int position = -1;
        #endregion

        #region private properties


        #endregion

        #region private methods

        void log(string s)
        {
            Log?.Invoke(s);
        }

       

        public bool Push(char c, bool throwexception=true)
        {
          //  log($"Pushing '{c}' value:{(int)c}\n");
            if (pushed)
            {
                if (('\uffff' == pushchar)  && ('\uffff' ==c))
                {
                    return true;
                }
                if (throwexception)
                {
                    throw new Exception($"can't push when buffer is already occupied: it has '{pushchar}' value:{(int)pushchar}, trying to push '{c}' value {(int)c}");
                }
                else
                {
                    return false;
                }
                
            }
            pushed = true;
            pushchar = c;
            return true;
        }

       
        bool Next(out char c)
        {
            if (pushed)
            {
             //   log($"popping '{pushchar}' value:{(int)pushchar}\n");
                pushed = false;
                c = pushchar;
                pushchar = '\0';
                if ('\uffff' == c)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            position++;
            int i = streamBeingParsed.Read();
            if (-1 == i)
            {
                Push((char)i);
            }
                
            c = (char)i;
            return true;
            
        }

  
   
        private string Transition(char c)   
        {
            
            char nextc = Peek();
            log($"Parsing '{c}' value:{(int)c,4} position:{position,5} next:'{nextc}'");
            State s;
            bool found = StateMachine.TryGetValue(currentState, out s);
            if (found)
            {
              
                log($"Transition:{s.Name,20}->");
                StateTransition st = s.Execute(s,c);
                log($"{st?.NewState??"NULL",20}\n");
                if (null != st?.ElementFound)
                {
                 //   log($"Consumed before push: '{unconsumedCharacters}'\n");
                    Push(c);
                 //   log($"Consumed after push: '{unconsumedCharacters}'\n");

                   // log($"data:'{st.ElementFound.Data}'");
                    collectedCharacters.Append(unconsumedCharacters);
                    unconsumedCharacters.Clear();
                    if (st.ElementFound is ParsedElement pe)
                    {
                        log($"ParsedElement:{pe.Data}\n");
                        parsedElements.Add(pe);
                        ElementParsed?.Invoke(pe);
                        
                    }
                    if (st.ElementFound is ParsedToken pt)
                    {
                        log($"ParsedToken:{pt.Data}\n");
                        parsedTokens.Add(pt);
                        TokenParsed?.Invoke(pt);
                    }
                    if (st.ElementFound is ParsedOperator po)
                    {
                        log($"ParsedOperator:{po.Data}\n");
                        OperatorParsed?.Invoke(po);
                    }
                   
                }
                else
                {
                    unconsumedCharacters.Append(c);
                }
                return st?.NewState??"";
            }
            else
            {
                throw new Exception($"State '{currentState}' not found in stateMachine");
            }
        }

        void privateParse()
        {
            char c;
            while(Next(out c))
            {
                currentState = Transition(c);
            }
        }
        #endregion



 
        #region public properties

        public System.IO.TextReader StreamBeingParsed { get { return streamBeingParsed; } }

        public bool IsParsed { get { return parsed; } }

        public  IReadOnlyList<ParsedElement> ParsedElements
        {
            get
            {
                return parsedElements;
            }
            
        }

        public  IReadOnlyList<ParsedToken> ParsedTokens
        {
            get
            {
                return parsedTokens;
            }
        }

        public string UnconsumedData { get {
                
                    return unconsumedCharacters.ToString();
               
            } }

        public Action<string> Log { get; set; }

        #endregion public properties
 
        #region public events

        public event Action<ParsedElement> ElementParsed;
        public event Action<ParsedToken> TokenParsed;
        public event Action<ParsedOperator> OperatorParsed;

        #endregion

        #region public methods

        public void Reset(string s)
        {
            Reset(new System.IO.StringReader(s));
        }
        public void Reset(System.IO.TextReader r)
        {
            parsed = false;
            parsing = false;
            streamBeingParsed = r;
            collectedCharacters.Clear();
            unconsumedCharacters.Clear();
            parsedElements.Clear();
            parsedTokens.Clear();
            currentState = "";
            pushed = false;
            pushchar = '\0';
            position = -1;
        }

        public bool ParseAll()
        {
            lock (this)
            {
                if (!parsing)
                {
                    parsed = true;
                    parsing = true;
                    privateParse();
                    return true;
                }
                else
                {
                    return false; ;
                }
            }

        }

        public char ParseNext()
        {
            lock (this)
            {
                if (parsed)
                {
                    return '\0';
                }
                if (!parsing)
                {
                    parsing = true;
                    
                }
                
            }
            char c;
            if ( Next(out c))
            {
                currentState = Transition(c);
                return c;
            }
            return '\0';
            
        }

        public State CreateState(string StateName,  Func<State, char, StateTransition> ExecuteFunction)
        {
            return new State()
            {
                Name    = StateName,
                Execute = ExecuteFunction,
                theParser = this ,                
            };
        }


        public char Peek()
        {
            bool b;
            return Peek(out b);
        }
        public char Peek(out bool b)
        {
            if (pushed)
            {
                b = true;
                return pushchar;
            }
            int i = streamBeingParsed.Peek();
            if (-1 == i)
            {
                b = false; return '\uffff';
            }
            else
            {
                b = true; return (char)i; ;
            }
        }

        public bool AddState(State newState, bool throwException=true)
        {
            if (StateMachine.ContainsKey(newState.Name))
            {
                if (throwException)
                {
                throw new Exception($"StateMachine already has the state '{newState.Name}'");
                }
                else
                {
                    return false;
                }
            }
            StateMachine.Add(newState.Name, newState);
            return true;

        }

        public void AddStates(params State[] states)
        {
            foreach (State s in states)
            {
                AddState(s);
            }
        }

        public bool InitializeStates(IEnumerable<State> states)
        {
            StateMachine = states.ToDictionary(k => k.Name);
            return true;
        }

        #endregion methods

   


    }
}
