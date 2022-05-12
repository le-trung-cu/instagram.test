using System.Text;

namespace Shared
{
    public class Base53
    {
        private string _base53;
        private int _value;

        private static readonly char[] chars =  {
              '_',
              'a', 'b', 'c', 'd', 'e', 'f', 'g',
              'h', 'i', 'j', 'k', 'l', 'm', 'n',
              'o', 'p', 'q', 'r', 's', 't', 'u',
              'v', 'w', 'x', 'y', 'z',
              'A', 'B', 'C', 'D', 'E', 'F', 'G',
              'H', 'I', 'J', 'K', 'L', 'M', 'N',
              'O', 'P', 'Q', 'R', 'S', 'T', 'U',
              'V', 'W', 'X', 'Y', 'Z',
        };

        private static int mapCharToInt(char character)
        {
            return character switch
            {
                '_' => 0,
                (>= 'a') and (<= 'z') => 1 + character - 'a',
                (>= 'A') and (<= 'Z') => 27 + character - 'A',
                _ => throw new ArgumentOutOfRangeException(nameof(character)),
            };
        }

        public Base53(string base53)
        {
            _base53 = base53;
            _value = convertBase53ToInt(base53);
        }
        public Base53(int value)
        {
            _value = value;
            _base53 = converToBase53(value);
        }
        public int Value => _value;
        public string Slug => _base53;
        public override string ToString() => _base53 ?? "";

        private int convertBase53ToInt(string base53)
        {
            var mods = base53.ToCharArray();
            var soChia = 0;
            for (var index = 0; index < mods.Length; index++)
            {
                soChia = soChia*53 + mapCharToInt(mods[index]);
            }
            return soChia;
        }

        private string converToBase53(int value)
        {
            if(value == 0)
            {
                return "_";
            }
            var mods = new Stack<int>();
            var nguyen = value;
            while(nguyen != 0)
            {
                var du = nguyen % 53;
                nguyen = nguyen / 53;
                mods.Push(du);
            }

            return mods.Aggregate(new StringBuilder(), (a, b) => a.Append(chars[b])).ToString();
        }
    }
}
