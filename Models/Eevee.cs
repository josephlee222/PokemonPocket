namespace PokemonPocket
{
    public class Eevee:Pokemon
    {
        public Eevee(int _Hp, int _Exp)
        {
            Name = "Eevee";
            Hp = _Hp;
            Exp = _Exp;
            Skill = "Run Away";
            Damage = 20;
            EvolveTo = "Sylveon";
            NoToEvolve = 3;
        }

        public int calculateDamage()
        {
            return Damage * 2;
        }
    }
}
