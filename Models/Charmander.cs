namespace PokemonPocket
{
    public class Charmander : Pokemon
    {
        public Charmander(int Hp, int Exp) : base(Hp, Exp)
        {
            Name = "Charmander";
            Skill = "Solar Power";
            Damage = 15;
        }

        public void calculateDamage(int damage_taken)
        {
            this.Hp = this.Hp - (damage_taken * 3);
        }
    }
}
