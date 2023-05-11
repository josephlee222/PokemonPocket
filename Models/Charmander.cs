namespace PokemonPocket
{
    public class Charmander : Pokemon
    {
        public Charmander(int _Hp, int _Exp)
        {
            Name = "Charmander";
            Hp = _Hp;
            Exp = _Exp;
            Skill = "Solar Power";
            Damage = 15;
            EvolveTo = "Charmeleon";
            NoToEvolve = 1;
        }

        public int calculateDamage()
        {
            return Damage * 3;
        }
    }
}
