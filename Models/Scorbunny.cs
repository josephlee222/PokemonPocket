namespace PokemonPocket
{
    public class Scorbunny:Pokemon
    {
        public Scorbunny(int Hp, int Exp) : base(Hp, Exp)
        {
            Name = "Scorbunny";
            Skill = "Libero";
            Damage = 20;
        }

        public void calculateDamage(int damage_taken)
        {
            this.Hp = this.Hp - (damage_taken * 2);
        }
    }
}
