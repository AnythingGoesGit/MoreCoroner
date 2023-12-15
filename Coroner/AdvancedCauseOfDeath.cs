using System;
using System.Collections.Generic;
using GameNetcodeStuff;

namespace Coroner
{
    class AdvancedDeathTracker
    {
        public const int PLAYER_CAUSE_OF_DEATH_DROPSHIP = 300;

        public static readonly string[] FUNNY_NOTES = {
            "The goofiest goober.",
            "The cutest employee.",
            "Had the most fun.",
            "Had the least fun.",
            "The bravest employee.",
            "Did a sick flip.",
            "Stubbed their toe.",
            "The most likely to die next time.",
            "The least likely to die next time.",
            "Dislikes smoke.",
            "A team player.",
            "A real go-getter.",
            "Ate the most snacks.",
            "Passed GO and collected $200.",
            "Got freaky on a Friday night.",
            "I think this one's a serial killer.",
            "Perfectly unremarkable.",
            "Hasn't called their mother in a while.",
            "Has IP address 127.0.0.1.",
            "Secretly a lizard",
            "Believed they could fly.",
            "Thought they had nine lives.",
            "Believes in Santa Claus.",
            "Still uses a flip phone.",
            "Can't cook instant noodles right.",
            "Thinks the Earth is flat.",
            "Still afraid of the dark.",
            "Always the first in line for coffee.",
            "Lost a bet with a parrot.",
            "Can't remember their passwords.",
            "Secretly a superhero on weekends.",
            "Tried to high-five a mirror.",
            "Thinks pineapple belongs on pizza.",
            "Never finishes their... ",
            "Talks to plants.",
            "Has a pet rock named Steve.",
            "Wanted to be an astronaut.",
            "Sleeps with a night light.",
            "Once got stuck in a revolving door.",
            "Has a collection of rubber ducks.",
            "Dances when no one is watching."
        };
        private static readonly Dictionary<int, AdvancedCauseOfDeath> PlayerCauseOfDeath = new Dictionary<int, AdvancedCauseOfDeath>();
        private static readonly Dictionary<int, string> PlayerNotes = new Dictionary<int, string>();

        public static void ClearDeathTracker()
        {
            PlayerCauseOfDeath.Clear();
            PlayerNotes.Clear();
        }

        public static void SetCauseOfDeath(int playerIndex, AdvancedCauseOfDeath causeOfDeath, bool broadcast = true)
        {
            PlayerCauseOfDeath[playerIndex] = causeOfDeath;
            if (broadcast) DeathBroadcaster.BroadcastCauseOfDeath(playerIndex, causeOfDeath);
        }

        public static void SetCauseOfDeath(int playerIndex, CauseOfDeath causeOfDeath, bool broadcast = true)
        {
            SetCauseOfDeath(playerIndex, ConvertCauseOfDeath(causeOfDeath), broadcast);
        }

        public static void SetCauseOfDeath(PlayerControllerB playerController, CauseOfDeath causeOfDeath, bool broadcast = true)
        {
            SetCauseOfDeath((int)playerController.playerClientId, ConvertCauseOfDeath(causeOfDeath), broadcast);
        }

        public static void SetCauseOfDeath(PlayerControllerB playerController, AdvancedCauseOfDeath causeOfDeath, bool broadcast = true)
        {
            SetCauseOfDeath((int)playerController.playerClientId, causeOfDeath, broadcast);
        }

        public static AdvancedCauseOfDeath GetCauseOfDeath(int playerIndex)
        {
            PlayerControllerB playerController = StartOfRound.Instance.allPlayerScripts[playerIndex];

            return GetCauseOfDeath(playerController);
        }

        public static AdvancedCauseOfDeath GetCauseOfDeath(PlayerControllerB playerController)
        {
            if (!PlayerCauseOfDeath.ContainsKey((int)playerController.playerClientId))
            {
                Plugin.Instance.PluginLogger.LogInfo($"Player {playerController.playerClientId} has no custom cause of death stored! Using fallback...");
                return GuessCauseOfDeath(playerController);
            }
            else
            {
                Plugin.Instance.PluginLogger.LogInfo($"Player {playerController.playerClientId} has custom cause of death stored! {PlayerCauseOfDeath[(int)playerController.playerClientId]}");
                return PlayerCauseOfDeath[(int)playerController.playerClientId];
            }
        }

        public static AdvancedCauseOfDeath GuessCauseOfDeath(PlayerControllerB playerController)
        {
            if (playerController.isPlayerDead)
            {
                if (IsHoldingJetpack(playerController))
                {
                    if (playerController.causeOfDeath == CauseOfDeath.Gravity)
                    {
                        return AdvancedCauseOfDeath.Player_Jetpack_Gravity;
                    }
                    else if (playerController.causeOfDeath == CauseOfDeath.Blast)
                    {
                        return AdvancedCauseOfDeath.Player_Jetpack_Blast;
                    }
                }

                return ConvertCauseOfDeath(playerController.causeOfDeath);
            }
            else
            {
                return AdvancedCauseOfDeath.Unknown;
            }
        }

        public static bool IsHoldingJetpack(PlayerControllerB playerController)
        {
            var heldObjectServer = playerController.currentlyHeldObjectServer;
            if (heldObjectServer == null) return false;
            var heldObjectGameObject = heldObjectServer.gameObject;
            if (heldObjectGameObject == null) return false;
            var heldObject = heldObjectGameObject.GetComponent<GrabbableObject>();
            if (heldObject == null) return false;

            if (heldObject is JetpackItem)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static AdvancedCauseOfDeath ConvertCauseOfDeath(CauseOfDeath causeOfDeath)
        {
            switch (causeOfDeath)
            {
                case CauseOfDeath.Unknown:
                    return AdvancedCauseOfDeath.Unknown;
                case CauseOfDeath.Bludgeoning:
                    return AdvancedCauseOfDeath.Bludgeoning;
                case CauseOfDeath.Gravity:
                    return AdvancedCauseOfDeath.Gravity;
                case CauseOfDeath.Blast:
                    return AdvancedCauseOfDeath.Blast;
                case CauseOfDeath.Strangulation:
                    return AdvancedCauseOfDeath.Strangulation;
                case CauseOfDeath.Suffocation:
                    return AdvancedCauseOfDeath.Suffocation;
                case CauseOfDeath.Mauling:
                    return AdvancedCauseOfDeath.Mauling;
                case CauseOfDeath.Gunshots:
                    return AdvancedCauseOfDeath.Gunshots;
                case CauseOfDeath.Crushing:
                    return AdvancedCauseOfDeath.Crushing;
                case CauseOfDeath.Drowning:
                    return AdvancedCauseOfDeath.Drowning;
                case CauseOfDeath.Abandoned:
                    return AdvancedCauseOfDeath.Abandoned;
                case CauseOfDeath.Electrocution:
                    return AdvancedCauseOfDeath.Electrocution;
                default:
                    return AdvancedCauseOfDeath.Unknown;
            }
        }

        public static string StringifyCauseOfDeath(CauseOfDeath causeOfDeath)
        {
            return StringifyCauseOfDeath(ConvertCauseOfDeath(causeOfDeath), Plugin.RANDOM);
        }

        public static string StringifyCauseOfDeath(AdvancedCauseOfDeath? causeOfDeath)
        {
            return StringifyCauseOfDeath(causeOfDeath, Plugin.RANDOM);
        }

        public static string StringifyCauseOfDeath(AdvancedCauseOfDeath? causeOfDeath, Random random)
        {
            var result = SelectCauseOfDeath(causeOfDeath);
            if (result.Length == 1) return result[0];
            else return result[random.Next(result.Length)];
        }

        public static string[] SelectCauseOfDeath(AdvancedCauseOfDeath? causeOfDeath)
        {
            if (causeOfDeath == null) return FUNNY_NOTES;

            switch (causeOfDeath)
            {
                case AdvancedCauseOfDeath.Bludgeoning:
                    return new[] {
                      "Bludgeoned to death.",
                      "Met the business end of a hammer.",
                      "Learned that gravity and rocks don't mix.",
                      "Discovered why pillows are safer than bricks.",
                      "Took a hard hit, literally.",
                      "Found out hammers aren't just for nails.",
                      "Tried to catch a falling anvil.",
                      "Wrestled with a rock and the rock won.",
                      "Received a heavy dose of blunt force trauma.",
                      "Had an unexpected rendezvous with a bat."
                };
                case AdvancedCauseOfDeath.Gravity:
                    return new[] {
                      "Fell to their death.",
                      "Fell off a cliff.",
                      "Discovered flying isn't for everyone.",
                      "Took a dive, forgot to stop.",
                      "Learned that what goes up must come down.",
                      "Gravity: 1, Common Sense: 0.",
                      "Had a falling out with the ground.",
                      "Tried skydiving without a parachute.",
                      "Did not stick the landing.",
                      "Was a victim of the law of gravity."
                };
                case AdvancedCauseOfDeath.Blast:
                    return new[] {
                      "Went out with a bang.",
                      "Exploded.",
                      "Was blown to smithereens.",
                      "Tried to juggle grenades.",
                      "Learned the hard way that red barrels explode.",
                      "Found out bombs are not toys.",
                      "Misunderstood the 'keep away from fire' warning.",
                      "Had a blast, but not in a good way.",
                      "Went out in a blaze of glory.",
                      "Had an explosive personality."
                };
                case AdvancedCauseOfDeath.Strangulation:
                    return new[] {
                      "Strangled to death.",
                      "Tried to swallow a boa constrictor.",
                      "Underestimated the strength of a necktie.",
                      "Took a chokehold way too seriously.",
                      "Tried a scarf, found it a bit too tight.",
                      "Played a deadly game of tug-o-war with a python.",
                      "Tried to deep throat a rope.",
                      "Realized too late that 'breath play' is not for everyone.",
                      "Found out the hard way that neck massages can be deadly.",
                      "Discovered that some necklaces are too tight."
                };
                case AdvancedCauseOfDeath.Suffocation:
                    return new[] {
                      "Suffocated to death.",
                      "Forgot how to breathe.",
                      "Played hide and seek in a vacuum.",
                      "Took a deadly nap in a plastic bag.",
                      "Tried to hold their breath indefinitely.",
                      "Got a little too intimate with a pillow.",
                      "Misplaced their oxygen.",
                      "Found out that vacuum cleaners are not for internal use.",
                      "Thought they could breathe underwater without equipment.",
                      "Didn't believe in the concept of breathing."
                };
                case AdvancedCauseOfDeath.Mauling:
                    return new[] {
                      "Mauled to death.",
                      "Tried to pet a bear.",
                      "Played fetch with a lion.",
                      "Thought they could outrun a cheetah.",
                      "Discovered too late that 'do not feed the animals' was a suggestion.",
                      "Tried to take a selfie with a tiger.",
                      "Decided to go for a midnight stroll in a wolf-infested forest.",
                      "Realized too late that 'playing dead' doesn't work with grizzly bears.",
                      "Found out the hard way that you can't tame a wild beast with a steak.",
                      "Got a little too close to nature."
                };
                case AdvancedCauseOfDeath.Gunshots:
                    return new[] {
                      "Shot to death by a turret.",
                      "Caught a bullet with their name on it.",
                      "Played dodgeball with bullets and lost.",
                      "Realized they weren't faster than a speeding bullet.",
                      "Learned that gunfights are not like the movies.",
                      "Found out what 'bullet time' really means.",
                      "Discovered that shooting stars can be lethal.",
                      "Took 'biting the bullet' too literally.",
                      "Had a fatal attraction to flying lead.",
                      "Learned the hard way that bullets are bad for health."
                };
                case AdvancedCauseOfDeath.Crushing:
                    return new[] {
                      "Crushed to death.",
                      "Had a smashing time, literally.",
                      "Played rock-paper-scissors with a boulder.",
                      "Discovered that walls can hug too tightly.",
                      "Found out that heavy lifting can be fatal.",
                      "Learned that ceilings can drop without notice.",
                      "Got a bit too close to a compactor.",
                      "Tried to fight gravity, and the floor won.",
                      "Experienced the squeeze of a lifetime.",
                      "Learned that sometimes the world weighs you down."
                };
                case AdvancedCauseOfDeath.Drowning:
                    return new[] {
                      "Drowned to death.",
                      "Took water aerobics too far.",
                      "Learned swimming the hard way.",
                      "Thought they could breathe underwater.",
                      "Got lost in the deep end.",
                      "Discovered the ocean's bottom isn't a fun visit.",
                      "Played mermaid/merman without the tail.",
                      "Went for a swim in cement shoes.",
                      "Underestimated the power of a puddle.",
                      "Had a wet and deadly encounter."
                };
                case AdvancedCauseOfDeath.Abandoned:
                    return new[] {
                      "Abandoned by their coworkers.",
                      "Learned that loneliness can be fatal.",
                      "Found out that being left behind wasn't a game.",
                      "Discovered that solitude can kill.",
                      "Got ghosted by their survival team.",
                      "Realized too late that no man is an island.",
                      "Was the last one at the party... forever.",
                      "Tried to solo a team sport.",
                      "Learned that 'alone time' should be limited.",
                      "Found out the hard way that there's no 'I' in team."
                };
                case AdvancedCauseOfDeath.Electrocution:
                    return new[] {
                      "Electrocuted to death.",
                      "Got a shocking surprise.",
                      "Tried to conduct electricity with their body.",
                      "Discovered that toasters and bathtubs don't mix.",
                      "Learned that power lines aren't for tightrope walking.",
                      "Had an electrifying dance with death.",
                      "Decided to chew on a power cord.",
                      "Found out lightning does strike twice.",
                      "Had a hair-raising experience with high voltage.",
                      "Received a jolt to the heart, and they're to blame."
                };
                case AdvancedCauseOfDeath.Kicking:
                    return new[] {
                      "Kicked to death.",
                      "Got a kick out of life, literally.",
                      "Learned that high-kicks can be deadly.",
                      "Discovered that shoes can pack a punch.",
                      "Faced the wrath of a thousand angry legs.",
                      "Took a boot to the head one too many times.",
                      "Received the ultimate kick-off.",
                      "Realized that being kicked around is more than an expression.",
                      "Became a victim of a lethal leg day.",
                      "Learned the hard way that some kicks are not for sport."
                };

                case AdvancedCauseOfDeath.Enemy_Bracken:
                    return new[] {
                      "Had their neck snapped by a Bracken.",
                      "Stared at a Bracken too long.",
                      "Hugged a Bracken. It didn't hug back.",
                      "Played peek-a-boo with a Bracken. Lost terribly.",
                      "Tried to make a Bracken smile.",
                      "Thought 'Bracken' was just a weird tree.",
                      "Gave a Bracken a stern talking to.",
                      "Brought a knife to a Bracken fight.",
                      "Tried to outstretch a Bracken.",
                      "Found out Brackens aren't vegetarians."
                };
                case AdvancedCauseOfDeath.Enemy_EyelessDog:
                    return new[] {
                      "Got caught using a mechanical keyboard.",
                      "Was eaten by an Eyeless Dog.",
                      "Forgot to 'speak softly' around an Eyeless Dog.",
                      "Wasn't quiet around an Eyeless Dog.",
                      "Played fetch and became the stick.",
                      "Tried to teach an Eyeless Dog new tricks.",
                      "Thought 'Eyeless' meant 'harmless'.",
                      "Learned Eyeless Dogs don't play dead.",
                      "Whistled in the dark. It came running.",
                      "Misjudged the bite of an Eyeless Dog."
                };
                case AdvancedCauseOfDeath.Enemy_ForestGiant:
                    return new[] {
                      "Swallowed whole by a Forest Giant.",
                      "Played hide-and-seek with a Forest Giant.",
                      "Tried to climb a Forest Giant. Fell.",
                      "Was the small spoon to a Forest Giant.",
                      "Went nature walking in tall-tall grass.",
                      "Gave a Forest Giant a flower. Got a tree.",
                      "Thought 'Forest Giant' was a metaphor.",
                      "Got stepped on during a Forest Giant's stroll.",
                      "Tried to outrun a Forest Giant's appetite.",
                      "Learned that Forest Giants don't do handshakes."
                };
                case AdvancedCauseOfDeath.Enemy_CircuitBees:
                    return new[] {
                      "Electro-stung to death by Circuit Bees.",
                      "Thought Circuit Bees made honey. They made pain.",
                      "Tried to swat a Circuit Bee. Bad idea.",
                      "Disturbed a Circuit Beehive. Lived shortly after.",
                      "Took a zap from the bee zap brigade.",
                      "Wore flower-scented cologne near Circuit Bees.",
                      "Learned that Circuit Bees don't do buzz-offs.",
                      "Caught in the crossfire of a Circuit Bee buzzsaw.",
                      "Tried to catch a Circuit Bee. It caught back.",
                      "Discovered the true meaning of 'shock and awe'."
                };
                case AdvancedCauseOfDeath.Enemy_GhostGirl:
                    return new[] {
                      "Died a mysterious death.",
                      "???",
                      "Played hide-and-seek with a Ghost Girl.",
                      "Tried to exorcise a Ghost Girl. Got exercised.",
                      "Became part of a Ghost Girl's haunting routine.",
                      "Misunderstood the concept of a 'spiritual encounter'.",
                      "Thought Ghost Girl was just a regular girl.",
                      "Learned that Ghost Girls don't like small talk.",
                      "Brought a Ouija board to a Ghost Girl fight.",
                  "Received a ghoulish goodbye."
                };
                case AdvancedCauseOfDeath.Enemy_EarthLeviathan:
                    return new[] {
                      "Swallowed whole by an Earth Leviathan.",
                      "Thought 'Earth Leviathan' was an exotic plant.",
                      "Tried to outdig an Earth Leviathan.",
                      "Learned Earth Leviathans don't play fetch.",
                      "Mistook an Earth Leviathan for a hill.",
                      "Got a one-way ticket to the Earth's core.",
                      "Found out that Earth Leviathans don't do piggybacks.",
                      "Tried to ride the Earth Leviathan express.",
                      "Learned that Earth Leviathans aren't herbivores.",
                      "Discovered the Earth Leviathan's insatiable appetite."
                };
                case AdvancedCauseOfDeath.Enemy_BaboonHawk:
                    return new[] {
                      "Was eaten by a Baboon Hawk.",
                      "Was mauled by a Baboon Hawk.",
                      "Mistook a Baboon Hawk for a friendly parrot.",
                      "Tried to outfly a Baboon Hawk.",
                      "Learned that Baboon Hawks don't do tricks.",
                      "Got a peck on the cheek, Baboon Hawk style.",
                      "Thought a Baboon Hawk was a new yoga pose.",
                      "Offered a Baboon Hawk a banana. Big mistake.",
                      "Played chicken with a Baboon Hawk. Lost.",
                      "Tried to enter a no-fly zone. Baboon Hawk disagreed."
                };
                case AdvancedCauseOfDeath.Enemy_Jester:
                    return new[] {
                      "Was the butt of the Jester's joke.",
                      "Got pranked by the Jester.",
                      "Got popped like a weasel.",
                      "Laughed to death by a Jester's prank.",
                      "Took a Jester seriously. Fatal error.",
                      "Played cards with a Jester. Dealt a dead hand.",
                      "Tried to outjoke a Jester.",
                      "Became the punchline of a deadly joke.",
                      "Received a deadly pie in the face.",
                      "Thought the Jester was just clowning around."
                };
                case AdvancedCauseOfDeath.Enemy_CoilHead:
                    return new[] {
                      "Got in a staring contest with a Coil Head.",
                      "Lost a staring contest with a Coil Head.",
                      "Tried to coil up with a Coil Head.",
                      "Learned that Coil Heads have a tight grip.",
                      "Thought a Coil Head was just misunderstood.",
                      "Tried to untangle a Coil Head's thoughts.",
                      "Played 'Simon Says' with a Coil Head.",
                      "Got wrapped up in a Coil Head's embrace.",
                      "Stared into the abyss, Coil Head stared back.",
                      "Tried to headbutt a Coil Head."
                };
                case AdvancedCauseOfDeath.Enemy_SnareFlea:
                    return new[] {
                      "Was suffocated by a Snare Flea.",
                      "Got caught by a Snare Flea's hug.",
                      "Learned that Snare Fleas aren't cuddly.",
                      "Tried to outjump a Snare Flea.",
                      "Flea circus audition turned deadly.",
                      "Found out Snare Fleas don't play fetch.",
                      "Discovered the Snare Flea's sticky situation.",
                      "Thought they could squash a Snare Flea.",
                      "Played leapfrog with a Snare Flea.",
                      "Learned that Snare Fleas have a tight grip."
                };
                case AdvancedCauseOfDeath.Enemy_Hygrodere:
                    return new[] {
                      "Was absorbed by a Hygrodere.",
                      "Got lost in the sauce.",
                      "Tried to moisturize with a Hygrodere.",
                      "Went for a dip in a Hygrodere pond.",
                      "Got a little too absorbed in nature.",
                      "Learned that Hygroderes aren't for hydration.",
                      "Mistook a Hygrodere for a waterbed.",
                      "Tried to bottle a Hygrodere's essence.",
                      "Learned that Hygroderes don't do splash fights.",
                      "Faced the Hygrodere's engulfing embrace."
                };
                case AdvancedCauseOfDeath.Enemy_HoarderBug:
                    return new[] {
                      "Was hoarded by a Hoarder Bug.",
                      "Became part of a Hoarder Bug's collection.",
                      "Learned that Hoarder Bugs don't share.",
                      "Tried to declutter a Hoarder Bug's nest.",
                      "Mistook a Hoarder Bug for a treasure chest.",
                      "Got a little too possessive with a Hoarder Bug.",
                      "Learned that Hoarder Bugs don't do trades.",
                      "Found out that Hoarder Bugs are avid collectors.",
                      "Thought Hoarder Bugs were just big fans.",
                      "Discovered the Hoarder Bug's greedy grasp."
                };
                case AdvancedCauseOfDeath.Enemy_SporeLizard:
                    return new[] {
                      "Was puffed by a Spore Lizard.",
                      "Inhaled more than just fresh air.",
                      "Thought Spore Lizard spores were perfume.",
                      "Learned that Spore Lizards don't do aromatherapy.",
                      "Tried to catch spores with their tongue.",
                      "Discovered the Spore Lizard's deadly dandruff.",
                      "Thought spores were just fancy confetti.",
                      "Tried to domesticate a Spore Lizard.",
                      "Learned that Spore Lizards are bad for allergies.",
                      "Mistook a Spore Lizard for a mushroom."
                };
                case AdvancedCauseOfDeath.Enemy_BunkerSpider:
                    return new[] {
                      "Ensnared in the Bunker Spider's web.",
                      "Got caught in a web of lies... and death.",
                      "Learned that Bunker Spiders don't do piggybacks.",
                      "Tried to outweb a Bunker Spider.",
                      "Thought web design was easy.",
                      "Discovered the Bunker Spider's sticky situation.",
                      "Played peek-a-boo with a Bunker Spider.",
                      "Mistook a Bunker Spider for a cuddly pet.",
                      "Learned that Bunker Spider silk isn't for knitting.",
                      "Got tangled in a deadly game of cat's cradle."
                };
                case AdvancedCauseOfDeath.Enemy_Thumper:
                    return new[] {
                      "Was ravaged by a Thumper.",
                      "Got thumped by a Thumper.",
                      "Learned that Thumpers don't play drums.",
                      "Tried to thump a Thumper.",
                      "Thought 'Thumper' was a dance move.",
                      "Played patty-cake with a Thumper.",
                      "Discovered the Thumper's seismic activity.",
                      "Mistook a Thumper for a friendly bunny.",
                      "Learned that Thumpers don't do high-fives.",
                      "Felt the Thumper's ground-shaking greeting."
                };

                case AdvancedCauseOfDeath.Enemy_MaskedPlayer_Wear:
                    return new[] {
                      "Nobody cared who they were until they put on the Mask.",
                      "Donned the Mask.",
                      "Learned the Mask comes with strings attached.",
                      "Found out the Mask wasn't just for show.",
                      "Wore the Mask, couldn't handle the role.",
                      "Discovered the Mask's fatal charm.",
                      "Tried to live a double life with the Mask.",
                      "Put on the Mask and exited stage left forever.",
                      "The Mask brought the curtains down on them.",
                      "The Mask's embrace was a tad too tight."
                };
                case AdvancedCauseOfDeath.Enemy_MaskedPlayer_Victim:
                    return new[] {
                      "Became a tragedy at the hands of the Mask.",
                      "Was killed by a Masked coworker.",
                      "Caught in the Mask's lethal performance.",
                      "Fell victim to the Mask's deadly allure.",
                      "Crossed paths with the Mask, and it was curtains.",
                      "Unmasked the Mask's deadly secret.",
                      "Learned that the Mask doesn't play favorites.",
                      "Was the final act in the Mask's play.",
                      "Tried to unmask a killer. It didn't go well.",
                      "Found out the Mask was no mere prop."
                };
                case AdvancedCauseOfDeath.Enemy_Nutcracker_Kicked:
                    return new[] {
                      "Got their nuts cracked by a Nutcracker.",
                      "Was kicked to death by a Nutcracker.",
                      "Learned that Nutcrackers have a mean kick.",
                      "Got a Nutcracker's footloose finale.",
                      "Discovered that Nutcrackers don't dance ballet.",
                      "Played soccer with a Nutcracker. Scored an own goal.",
                      "Thought Nutcrackers only cracked nuts. Was wrong.",
                      "Received the Nutcracker's signature move.",
                      "Had a cracking good time, in the worst way.",
                      "Tested the Nutcracker's kick. It passed."
                };
                case AdvancedCauseOfDeath.Enemy_Nutcracker_Shot:
                    return new[] {
                      "Was at the wrong end of a 21-gun salute.",
                      "Got shot by a Nutcracker.",
                      "Learned that Nutcrackers are sharpshooters.",
                      "Caught a Nutcracker's bullet with their teeth.",
                      "Faced the Nutcracker's firing squad.",
                      "Discovered that Nutcrackers have perfect aim.",
                      "Got a front-row seat to the Nutcracker's gun show.",
                      "Tried to dodge a Nutcracker's bullet. Tried.",
                      "Learned that Nutcrackers don't shoot blanks.",
                      "Was the target of the Nutcracker's wrath."
                };

                case AdvancedCauseOfDeath.Player_Jetpack_Gravity:
                    return new[] {
                      "Flew too close to the sun.",
                      "Ran out of fuel.",
                      "Misjudged their jetpack's altitude limit.",
                      "Tried to touch the sky, kissed the ground instead.",
                      "Found out that what goes up doesn't always come down gently.",
                      "Learned that jetpacks and gravity don't mix.",
                      "Took a nosedive into the law of gravity.",
                      "Ignored their jetpack's low fuel warning.",
                      "Tried to outfly gravity. Gravity won.",
                      "Jetpacked straight into an unexpected landing."
                };
                case AdvancedCauseOfDeath.Player_Jetpack_Blast:
                    return new[] {
                      "Turned into a firework.",
                      "Got blown up by bad piloting.",
                      "Flew high, then went out with a bang.",
                      "Learned that jetpacks can be volatile.",
                      "Took a jetpack joyride to a fiery end.",
                      "Ignited a jetpack-fueled explosion.",
                      "Had a blast-off that turned into a blast.",
                      "Learned to fly, then learned to explode.",
                      "Thought jetpack flame was just for show.",
                      "Missed the jetpack's user manual page on explosions."
                };
                case AdvancedCauseOfDeath.Player_Murder_Melee:
                    return new[] {
                      "Was the victim of a murder.",
                      "Got murdered.",
                      "Was bludgeoned to death by a coworker.",
                      "Felt the final hit of workplace violence.",
                      "Discovered the deadly side of office politics.",
                      "Learned that some coworkers are cutthroat, literally.",
                      "Was on the receiving end of a killer punchline.",
                      "Found out that the pen can be mightier than the sword.",
                      "Took team building exercises to a fatal level.",
                      "Learned that 'backstabber' is sometimes literal."
                };
                case AdvancedCauseOfDeath.Player_Murder_Shotgun:
                    return new[] {
                      "Was the victim of a murder.",
                      "Got murdered.",
                      "Was shot to death by a coworker.",
                      "Learned that shotguns are not for conflict resolution.",
                      "Was at the business end of office politics.",
                      "Discovered that some meetings are final.",
                      "Faced the ultimate 'you're fired' scenario.",
                      "Found out the conference room was a kill zone.",
                      "Took a shotgun shell seminar.",
                      "Received a lethal lesson in shotgun diplomacy."
                };
                case AdvancedCauseOfDeath.Player_Quicksand:
                    return new[] {
                      "Got stuck in quicksand.",
                      "Drowned in quicksand",
                      "Couldn't outpace the quicksand.",
                      "Found out that quicksand isn't a spa treatment.",
                      "Played in the sand, stayed forever.",
                      "Mistook quicksand for a mud bath.",
                      "Learned that quicksand plays for keeps.",
                      "Took a quicksand pit stop, permanently.",
                      "Tried to quickstep over quicksand.",
                      "Discovered that quicksand doesn't come with an escape key."
                };
                case AdvancedCauseOfDeath.Player_DepositItemsDesk:
                    return new[] {
                      "Received a demotion.",
                      "Was put on disciplinary leave.",
                      "Lost a fight with the returns desk.",
                      "Tried to deposit their soul. It was accepted.",
                      "Discovered that some deposits are final.",
                      "Learned the deadly cost of item returns.",
                      "Found out that the desk takes more than items.",
                      "Received a lethal receipt for their deposit.",
                      "Was permanently relieved of duty... and life.",
                      "Learned that the deposit desk has a no-refund policy."
                };
                case AdvancedCauseOfDeath.Player_Dropship:
                    return new[] {
                      "Couldn't wait for their items.",
                      "Got too impatient for their items.",
                      "Miscalculated the dropship's delivery speed.",
                      "Got an express delivery to the afterlife.",
                      "Found out that dropships don't do soft landings.",
                      "Learned that patience is more than a virtue; it's survival.",
                      "Rushed a dropship delivery, signed off permanently.",
                      "Discovered the fatal consequences of cutting in line.",
                      "Tried to intercept a dropship. Failed.",
                      "Learned that dropships have a deadly ETA."
                };
                case AdvancedCauseOfDeath.Player_StunGrenade:
                    return new[] {
                      "Was the victim of a murder.",
                      "Got a shocking end from a stun grenade.",
                      "Learned that stun grenades can be stunningly lethal.",
                      "Was shocked into the hereafter.",
                      "Had a stunningly brief encounter with a grenade.",
                      "Discovered the electrifying truth about stun grenades.",
                      "Was left stunned... forever.",
                      "Got a hair-raising, life-ending jolt.",
                      "Found out that some shocks are final.",
                      "Received a stunning exit cue."
                };

                // case AdvancedCauseOfDeath.Unknown:
                default:
                    return new[] {
                      "Most sincerely dead.",
                      "Died somehow.",
                      "Passed away under mysterious circumstances.",
                      "Left this world in an unspecified manner.",
                      "Ceased living in an indeterminate fashion.",
                      "Reached an uncertain end.",
                      "Met their demise through unknown means.",
                      "Expired due to reasons yet to be determined.",
                      "Shuffled off this mortal coil in a vague way.",
                      "Concluded their life's story with an enigmatic ellipsis."
                };
            }
        }

        internal static void SetCauseOfDeath(PlayerControllerB playerControllerB, object enemy_BaboonHawk)
        {
            throw new NotImplementedException();
        }
    }

    enum AdvancedCauseOfDeath
    {
        // Basic causes of death
        Unknown,
        Bludgeoning,
        Gravity,
        Blast,
        Strangulation,
        Suffocation,
        Mauling,
        Gunshots,
        Crushing,
        Drowning,
        Abandoned,
        Electrocution,
        Kicking, // New in v45

        // Custom causes (enemies)
        Enemy_BaboonHawk, // Also known as BaboonBird
        Enemy_Bracken, // Also known as Flowerman
        Enemy_CircuitBees, // Also known as RedLocustBees
        Enemy_CoilHead,  // Also known as SpringMan
        Enemy_EarthLeviathan, // Also known as SandWorm
        Enemy_EyelessDog, // Also known as MouthDog
        Enemy_ForestGiant,
        Enemy_GhostGirl, // Also known as DressGirl
        Enemy_Hygrodere, // Also known as Blob
        Enemy_Jester,
        Enemy_SnareFlea, // Also known as Centipede
        Enemy_SporeLizard, // Also known as Puffer
        Enemy_HoarderBug,
        Enemy_Thumper,
        Enemy_BunkerSpider,

        // Enemies from v45
        Enemy_MaskedPlayer_Wear, // Comedy mask
        Enemy_MaskedPlayer_Victim, // Comedy mask
        Enemy_Nutcracker_Kicked,
        Enemy_Nutcracker_Shot,

        // Custom causes (player)
        Player_Jetpack_Gravity,
        Player_Jetpack_Blast,
        Player_Quicksand,
        Player_Murder_Melee,
        Player_Murder_Shotgun,
        Player_DepositItemsDesk,
        Player_Dropship,
        Player_StunGrenade, // TODO: Implement this.
    }
}