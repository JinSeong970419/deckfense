namespace Deckfense
{
    public abstract class Spell
    {
        protected string skillId;

        public string SkillId { get { return skillId; } set { skillId = value; } }


        public abstract void Active();


        static public Spell Create(string spellId)
        {
            Spell skill = null;
            switch (spellId)
            {
                default:
                    {
                        break;
                    }

            }

            skill.skillId = spellId;
            return skill;
        }


    }
}
