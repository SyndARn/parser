using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace parser
{
    public class InputStreamer 
    {
        private StreamReader _reader;
        char[] buffer = new char[1];
        bool HasBeenRead;
        public InputStreamer(StreamReader reader)
        {
            _reader = reader;
        }

        public char Next()
        {
            if (HasBeenRead)
            {
                HasBeenRead = false;
                return buffer[0];
            }
            _reader.Read(buffer, 0, 1);
            return buffer[0];
        }

        public char Peek()
        {
            if (!HasBeenRead)
            {
                _reader.Read(buffer, 0, 1);
                HasBeenRead = true;
            }
            return buffer[0];
        }

        public bool Eos()
        {
            return _reader.EndOfStream;
        }

        public void Fail(string msg)
        {
            throw new Exception(msg);
        }
    }
}
