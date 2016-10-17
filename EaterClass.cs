using AntMe.English;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntMe.Player.Eater
{
    [Player(
        ColonyName = "Eater",
        FirstName = "",
        LastName = ""
    )]
    [Caste(
        Name = "Default",
        AttackModifier = 0,
        EnergyModifier = 0,
        LoadModifier = 0,
        RangeModifier = 0,
        RotationSpeedModifier = 0,
        SpeedModifier = 0,
        ViewRangeModifier = 0
    )]
    [Caste(
        Name = "Worker",
        AttackModifier = 0,
        EnergyModifier = 0,
        LoadModifier = 0,
        RangeModifier = 0,
        RotationSpeedModifier = 0,
        SpeedModifier = 0,
        ViewRangeModifier = 0
    )]
    [Caste(
        Name = "Eater",
        AttackModifier = -1,
        EnergyModifier = -1,
        LoadModifier = -1,
        RangeModifier = 0,
        RotationSpeedModifier = 0,
        SpeedModifier = 1,
        ViewRangeModifier = 2
    )]

    public class EaterClass : BaseAnt
    {


        #region Constants
        private static int NUMBER_OF_EATERS = 20;
        private static int NUMBER_OF_WORKERS = 80;
        private static int ANTHILLDISTANCE = 350;

        private static int MSG_COLLECT_SUGAR = 1;
        private static int RNG_COLLECT_SUGAR = ANTHILLDISTANCE;
        private static int MSG_EAT_SUGAR = 2;
        private static int RNG_EAT_SUGAR = 350;

        #endregion

        #region Caste

        /// <summary>
        /// Every time that a new ant is born, its job group must be set. You can 
        /// do so with the help of the value returned by this method.
        /// Read more: "http://wiki.antme.net/en/API1:ChooseCaste"
        /// </summary>
        /// <param name="typeCount">Number of ants for every caste</param>
        /// <returns>Caste-Name for the next ant</returns>
        public override string ChooseCaste(Dictionary<string, int> typeCount)
        {
            if (typeCount["Eater"] < NUMBER_OF_EATERS)
            {
                return "Eater";
            }
            return "Worker";
        }

        #endregion

        #region Movement

        /// <summary>
        /// If the ant has no assigned tasks, it waits for new tasks. This method 
        /// is called to inform you that it is waiting.
        /// Read more: "http://wiki.antme.net/en/API1:Waiting"
        /// </summary>
        public override void Waiting()
        {
            this.TurnByDegrees(RandomNumber.Number(-179, 180));
            this.GoForward();
        }

        /// <summary>
        /// This method is called when an ant has travelled one third of its 
        /// movement range.
        /// Read more: "http://wiki.antme.net/en/API1:GettingTired"
        /// </summary>
        public override void GettingTired()
        {
        }

        /// <summary>
        /// This method is called if an ant dies. It informs you that the ant has 
        /// died. The ant cannot undertake any more actions from that point forward.
        /// Read more: "http://wiki.antme.net/en/API1:HasDied"
        /// </summary>
        /// <param name="kindOfDeath">Kind of Death</param>
        public override void HasDied(KindOfDeath kindOfDeath)
        {
        }

        /// <summary>
        /// This method is called in every simulation round, regardless of additional 
        /// conditions. It is ideal for actions that must be executed but that are not 
        /// addressed by other methods.
        /// Read more: "http://wiki.antme.net/en/API1:Tick"
        /// </summary>
        public override void Tick()
        {
            if (this.Caste == "Eater") {
            } else
            {
                if( this.DistanceToAnthill >= ANTHILLDISTANCE)
                {
                    this.GoToAnthill();
                }
            }
        }

        #endregion

        #region Food

        /// <summary>
        /// This method is called as soon as an ant sees an apple within its 360° 
        /// visual range. The parameter is the piece of fruit that the ant has spotted.
        /// Read more: "http://wiki.antme.net/en/API1:Spots(Fruit)"
        /// </summary>
        /// <param name="fruit">spotted fruit</param>
        public override void Spots(Fruit fruit)
        {
        }

        /// <summary>
        /// This method is called as soon as an ant sees a mound of sugar in its 360° 
        /// visual range. The parameter is the mound of sugar that the ant has spotted.
        /// Read more: "http://wiki.antme.net/en/API1:Spots(Sugar)"
        /// </summary>
        /// <param name="sugar">spotted sugar</param>
        public override void Spots(Sugar sugar)
        {
            if (this.Destination == null)
            {
                this.GoToDestination(sugar);
            }
        }

        /// <summary>
        /// If the ant’s destination is a piece of fruit, this method is called as soon 
        /// as the ant reaches its destination. It means that the ant is now near enough 
        /// to its destination/target to interact with it.
        /// Read more: "http://wiki.antme.net/en/API1:DestinationReached(Fruit)"
        /// </summary>
        /// <param name="fruit">reached fruit</param>
        public override void DestinationReached(Fruit fruit)
        {
        }

        /// <summary>
        /// If the ant’s destination is a mound of sugar, this method is called as soon 
        /// as the ant has reached its destination. It means that the ant is now near 
        /// enough to its destination/target to interact with it.
        /// Read more: "http://wiki.antme.net/en/API1:DestinationReached(Sugar)"
        /// </summary>
        /// <param name="sugar">reached sugar</param>
        public override void DestinationReached(Sugar sugar)
        {
            if (this.Caste == "Eater")
            {
                if (this.DistanceToAnthill > ANTHILLDISTANCE && this.Destination == null)
                {
                    this.MakeMark(MSG_EAT_SUGAR, RNG_EAT_SUGAR);
                    this.Take(sugar);
                    this.Drop();
                    this.GoToDestination(sugar);
                }else if (this.DistanceToAnthill <= ANTHILLDISTANCE)
                {
                    this.GoAwayFrom(sugar);
                    this.MakeMark(MSG_COLLECT_SUGAR, RNG_COLLECT_SUGAR);
                }
            }
            else if (this.Caste == "Worker")
            {
                this.MakeMark(MSG_COLLECT_SUGAR, RNG_COLLECT_SUGAR);
                this.Take(sugar);
                this.GoToAnthill();
            }
        }

        #endregion

        #region Communication

        /// <summary>
        /// Friendly ants can detect markers left by other ants. This method is called 
        /// when an ant smells a friendly marker for the first time.
        /// Read more: "http://wiki.antme.net/en/API1:DetectedScentFriend(Marker)"
        /// </summary>
        /// <param name="marker">marker</param>
        public override void DetectedScentFriend(Marker marker)
        {
            if(this.Caste == "Eater")
            {
                if(marker.Information == MSG_EAT_SUGAR && this.Destination == null)
                {
                    this.GoToDestination(marker);
                }
            }else if (this.Caste == "Worker")
            {
                if(marker.Information == MSG_COLLECT_SUGAR && this.Destination == null)
                {
                    this.GoToDestination(marker);
                }
            }
        }

        /// <summary>
        /// Just as ants can see various types of food, they can also visually detect 
        /// other game elements. This method is called if the ant sees an ant from the 
        /// same colony.
        /// Read more: "http://wiki.antme.net/en/API1:SpotsFriend(Ant)"
        /// </summary>
        /// <param name="ant">spotted ant</param>
        public override void SpotsFriend(Ant ant)
        {
        }

        /// <summary>
        /// Just as ants can see various types of food, they can also visually detect 
        /// other game elements. This method is called if the ant detects an ant from a 
        /// friendly colony (an ant on the same team).
        /// Read more: "http://wiki.antme.net/en/API1:SpotsTeammate(Ant)"
        /// </summary>
        /// <param name="ant">spotted ant</param>
        public override void SpotsTeammate(Ant ant)
        {
        }

        #endregion

        #region Fight

        /// <summary>
        /// Just as ants can see various types of food, they can also visually detect 
        /// other game elements. This method is called if the ant detects an ant from an 
        /// enemy colony.
        /// Read more: "http://wiki.antme.net/en/API1:SpotsEnemy(Ant)"
        /// </summary>
        /// <param name="ant">spotted ant</param>
        public override void SpotsEnemy(Ant ant)
        {
            if (this.Caste == "Worker")
            {
                this.Drop();
                this.Attack(ant);
            }
        }

        /// <summary>
        /// Just as ants can see various types of food, they can also visually detect 
        /// other game elements. This method is called if the ant sees a bug.
        /// Read more: "http://wiki.antme.net/en/API1:SpotsEnemy(Bug)"
        /// </summary>
        /// <param name="bug">spotted bug</param>
        public override void SpotsEnemy(Bug bug)
        {
        }

        /// <summary>
        /// Enemy creatures may actively attack the ant. This method is called if an 
        /// enemy ant attacks; the ant can then decide how to react.
        /// Read more: "http://wiki.antme.net/en/API1:UnderAttack(Ant)"
        /// </summary>
        /// <param name="ant">attacking ant</param>
        public override void UnderAttack(Ant ant)
        {
            this.Drop();
            this.Attack(ant);
        }

        /// <summary>
        /// Enemy creatures may actively attack the ant. This method is called if a 
        /// bug attacks; the ant can decide how to react.
        /// Read more: "http://wiki.antme.net/en/API1:UnderAttack(Bug)"
        /// </summary>
        /// <param name="bug">attacking bug</param>
        public override void UnderAttack(Bug bug)
        {
        }

        #endregion
    }
}