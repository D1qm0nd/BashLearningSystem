namespace EncryptModule
{
    public class Cryptograph
    {
        private string _alphabet;

        private int[] _key;

        public char GetCharFromAlphaBet(int index)
        {
            return _alphabet[index % _alphabet.Length];
        }

        public Cryptograph(int[] key, string alphabet)
        {
            _key = key;
            _alphabet = alphabet;
        }

        public string Coding(string a)
        {
            string str = "";
            int k = 0;

            for (int i = 0; i < a.Length; i++)
            {
                for (int p = 0; p < _alphabet.Length; p++)
                    if (a[i] == _alphabet[p])
                        str += GetCharFromAlphaBet(p + _key[k]);
                if (k < _key.Length - 1) k++;
                else k = 0; 
            }

            return str;
        }

        public string DeCoding(string a)
        {
            string str = "";
            int k = 0;

            for (int i = 0; i < a.Length; i++)
            {
                for (int p = 0; p < _alphabet.Length; p++)
                {
                    if (a[i] == _alphabet[p])
                    {
                        if ((p - _key[k]) < 0)
                        {
                            str += GetCharFromAlphaBet((p + _alphabet.Length) - _key[k] % _alphabet.Length);
                        }
                        else
                        {
                            var index = (p - _key[k]);
                            if (index > 0)
                                str += GetCharFromAlphaBet(index);
                            else if (index < 0) str += GetCharFromAlphaBet(-index);
                            else str += GetCharFromAlphaBet(0);
                        }
                    }
                }

                if (k < _key.Length - 1) k++;
                else k = 0;
            }
            return str;
        }
    }
}
