namespace PokemonPocket
{
    public class Pikachu:Pokemon
    {
        public Pikachu(int _Hp, int _Exp)
        {
            Name = "Pikachu";
            Hp = _Hp;
            Exp = _Exp;
            Skill = "Lightning Bolt";
            Damage = 25;
            EvolveTo = "Raichu";
            NoToEvolve = 2;
        }

        public int calculateDamage()
        {
            return Damage * 1;
        }
    }
}
