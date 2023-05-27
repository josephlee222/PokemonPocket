namespace PokemonPocket
{
    public class Zeraora:Pokemon
    {
        public Zeraora(int Hp, int Exp) : base(Hp, Exp)
        {
            Name = "Zeraora";
            Skill = "Volt Absorb";
            Damage = 40;
        }

        public void calculateDamage(int damage_taken)
        {
            this.Hp = this.Hp - (damage_taken * 2);
        }
    }
}
