using System.Collections.Generic;

namespace Destiny.Constants
{
    public static class ConsumableConstants
    {
        #region Consumables
        public static readonly HashSet<int> ConsumablesMapleIDs = new HashSet<int>
        {
            2000000, //  Red Potion - A potion made out of red herbs.\nRecovers 50 HP.
            2000001, //  Orange Potion - A concentrated potion made out of red herbs.\nRecovers 150 HP.
            2000002, //  White Potion - A highly-concentrated potion made out of red herbs.\nRecovers 300 HP.
            2000003, //  Blue Potion - A potion made out of blue herbs.\nRecovers 100 MP.
            2000004, //  Elixir - A legendary potion.\nRecovers 50%  HP and 50% MP.
            2000005, //  Power Elixir - A legendary potion.\nRecovers all HP and MP.
            2000006, //  Mana Elixir - A legendary potion.\nRecovers around 300 MP.
            2000007, //  Red Pill - A pill of concentrated red potion, which restores 50 HP. You can carry more pills than potions because they're smaller
            2000008, //  Orange Pill - A pill of concentrated orange potion, which restores 150 HP. You can carry more pills than potions because they're smaller
            2000009, //  White Pill - A pill of concentrated white potion, which restores 300 HP. You can carry more pills than potions because they're smaller
            2000010, //  Blue Pill - A pill of concentrated blue potion, which restores 100 MP. You can carry more pills than potions because they're smaller
            2000011, //  Mana Elixir Pill - A pill of concentrated Mana Elixir, which restores 300 MP. You can carry more pills than potions because they're smaller
            2000012, //  Elixir - A legendary secret potion.\nRecovers 50% of HP and MP.
            2000013, //  Red Potion for Beginners - A potion made out of red herbs made especially for beginners. \nRecovers 40 HP.
            2000014, //  Blue Potion for Beginners - A potion made out of blue herbs made especially for beginners. \nRecovers 80 MP.
            2000015, //  Orange Potion for Beginners - A concentrated potion made out of red herbs.\nRecovers 150 HP.
            2000016, //  White Potion - A highly-concentrated potion made out of red herbs.\nRecovers 300 HP.
            2000017, //  Blue Potion - A potion made out of blue herbs.\nRecovers 100 MP.
            2000018, //  Mana Elixir - A legendary potion.\nRecovers around 300 MP.
            2000019, //  Power Elixir - A legendary potion.\nRecovers all HP and MP.
            2000020, //  Watermelon - A very ripe watermelon.\nRecovers about 1000 HP and 1000 MP.
            2000021, //  Ice Cream Pop - A tasty ice cream pop.\nRecovers around 2000 HP.
            2000022, //  Red Bean Sundae - Definitely lets you forget about the hot hot summer. \nRecovers around 2000 MP.
            2000023, //  Dexterity Potion - Adds quickness.\nAvoidability +5 for 3 min.
            2001000, //  Speed Potion - Increases speed.\nSpeed +8 for 3 min.
            2001001, //  Magic Potion - Increases magic attack.\nMagic Attack +5 for 3 min.
            2001002, //  Wizard Potion - Increases magic attack.\nMagic Attack +10 for 3 min.
            2002000, //  Warrior Potion - Increases attacking ability.\nAttack +5 for 3 min.
            2002001, //  Sniper Potion - Increases accuracy.\nAccuracy +5 for 5 min.
            2002002, //  Warrior Pill - A pill of concentrated warrior potion. Att. + 5 for 10 minutes
            2002003, //  Magic Pill - A pill of concentrated magic potion. Magic Att. + 5 for 10 minutes
            2002004, //  Sniper Pill - A pill of concentrated sniper potion. Accuracy + 10 for 10 minutes
            2002005, //  Dexterity Pill - A pill of concentrated dexterity potion. Avoidablity + 10 for 10 minutes
            2002006, //  Speed Pill - A pill of concentrated speed potion. Increased speed for 10 minutes
            2002007, //  Pain Reliever - A special pain reliever created in Omega Sector.nWeapon Def + 30 for 30 min.
            2002008, //  Elixir - Recovers both HP and MP by 50%.
            2002009, //  Power Elixir - Fully recovers HP and MP.
            2002010, //  Iron Body Medicine - A n enhanced pain reliever smuggled out of Omega Sector.n Def + 30 for 30 min. (Modification Request)
            2002011, //  Elpam Elixir - A rare, powerful elixir from Versal. Restores 90% HP & MP, and gives a boost of +5 Att and +40 Def for a duration of 15 minutes.
            2002012, //  Thief Elixir - A special elixir for Thieves. Gives +9 Accuracy and +15 Avoidability for 8 minutes.
            2002013, //  Warrior Elixir - A special elixir for Warriors. Gives +12 W. Att for 8 minutes.
            2002014, //  Wizard Elixir - A special elixir for Magicians. Gives +20 M.Att for 8 minutes.
            2002015, //  Archer Elixir - A special elixir for Bowmen. Gives +20 Avoidability for 8 minutes.
            2002016, //  Mana Bull - A special potion made in New Leaf City, recovers 60% MP.
            2002017, //  Honster - A special potion made in New Leaf City, recovers 60% HP.
            2002018, //  Ginseng Root - Pure ginseng extract that recovers 40% of HP and MP.
            2002019, //  Ginger Ale - Strong brewed Ginger Ale. Recovers 75% of HP and MP.
            2002020, //  Sorcerer Elixir - A rare potion perfect for Magicians. Recovers 1500 MP.
            2002021, //  Barbarian Elixir - A volatile potion mixed on the battlefield. Recovers 1500 HP.
            2002022, //  Ginger Ale - Strong brewed Ginger Ale. Recovers 75% of HP and MP.
            2002023, //  Stirge Signal - A makeshift device that looks like it's been duct-taped together.  Can be used to distract enemies... slightly.  [Gives +5 Avoidability for 20 minutes.]
            2002024, //  T-1337 Supercharger - A charger for an advanced model cyborg. Looks like it would give quite a shock to the system! [Gives +25 Weapon Attack, +60 Magic Attack for 20 minutes.]
            2002025, //  Ridley's Scroll of Defense - Why wear cumbersome armor when Ridley can provide the same protection, weightlessly and more comfortably!  (Legal Disclaimer: Effect is temporary) [Gives +100 Overall Defense for 10 minutes.]
            2002026, //  Apple - A red, ripe, and tasty apple.\nRecovers around 30 HP
            2002027, //  Meat - A tasty-looking meat. .\nRecovers around 100 HP.
            2002028, //  Egg - A nutritious egg.nImproves around 50 HP.
            2002029, //  Orange - A sweet, tasty orange.\nRecovers around 50 MP.
            2002030, //  Lemon - Very sour.\nRecovers around 150 MP.
            2010000, //  Honey - Fresh honey extracted from the beehive. \nRecovers around 30% of both HP and MP.
            2010001, //  Pot of Honey - A pot full of fresh honey.\nRecovers 50% of both HP and MP.
            2010002, //  Roger's Apple - A ripe, red apple.\nRecovers HP 30.nn#cTo eat Roger's Apple, simply double-click on it in your use inventory#.
            2010003, //  Green Apple - Sour and crunchy green apple.\nRecovers MP +30.
            2010004, //  Poisonous Mushroom - A very poisonous mushroom.
            2010005, //  Drake's Blood - Drake's blood. nAttack +8 for 5 min.
            2010006, //  Fairy's Honey - It's honey, the fairies' favorite.nAvoidability +10 for 5 min.
            2010007, //  Sap of Ancient Tree - Sap of a thousands-of-years-old tree. \nMagic Attack +10 for 5 min.
            2010009, //  Drake's Meat - Drake's meat.nWeapon Def. +10 for 5 min.
            2011000, //  Purified Honey - Fairies favorite purified honey.nAvoidability +40 for 5 min. (Modification Request)
            2012000, //  Salad - Made out of fresh vegetable.\nRecovers 200 MP.
            2012001, //  Fried Chicken - Well-fried chicken.\nRecovers around 200 HP.
            2012002, //  Cake - A cake full of sweetness. Recovers 100 for both HP and MP.
            2012003, //  Pizza - A freshly-baked pizza. Recovers 400 HP.
            2012004, //  Hamburger - A hamburger with bulgogi in it. Recovers 400 HP.
            2012005, //  Hot Dog - A hotdog with ketchup on it. Recovers 300 HP.
            2012006, //  Hot Dog Supreme - A huge hot dog. Recovers 500 HP.
            2020000, //  Dried Squid - Well-dried. Recovers 600 HP.
            2020001, //  Fat Sausage - Tastes great, and is quite nutritious. Recovers 1200 HP.
            2020002, //  Orange Juice - Pure OJ... Recovers 450 MP.
            2020003, //  Grape Juice - Used real grapes for this. Recovers 900 MP.
            2020004, //  W Ramen - A cup ramen with awesome soup.\nRecovers 40% of HP and MP..
            2020005, //  Melting Cheese - A mouth-watering cheese made out of fresh milk.\nRecovers 4000 HP.
            2020006, //  Reindeer Milk - Fresh milk squeezed out of a reindeer.\nRecovers 5000 HP.
            2020007, //  Sunrise Dew - Dew collected early morning. Recovers 4000 MP
            2020008, //  Sunset Dew - Dew collected late in the afternoon. Recovers 5000 MP.
            2020009, //  Cheesecake - MapleStory's 4th Anniversary Cake. Recovers 1200 HP and MP.
            2020010, //  Strawberry Cream Cake - MapleStory 4th Anniversary Cake. Recovers 1400 HP and MP.
            2020011, //  Chocolate Cream Cake - MapleStory 4th Anniversary Cake. Recovers 1600 HP and MP.
            2020012, //  Chocolate Cake - MapleStory 4th Anniversary Cake. Recovers 1800 HP and MP.
            2020013, //  Wedding Cake - A Wedding cake that coming  Recovers 2000 HP and MP.
            2020014, //  Nemi's Lunch Box - A fresh-made lunch box made by Nemi of Ludibrium for her dad, Kaho of the Toy Factory. Recovers 10 HP.
            2020015, //  White Chocolate - A very delicious, home-made white chocolate. Attack + 5 for 30 minutes.
            2020016, //  Dark Chocolate - A very delicious, home-made dark chocolate. Magic Attack + 5 for 30 minutes.
            2020017, //  Chocolate Basket - A basket full of delicious, home-made chocolate decorated with ribbons and marbles. Avoidability, speed and accuracy +10 each for 30 minutes.
            2020018, //  Pineapple Candy - A very sweet, home-made pineapple candy. Attack +5 for 30 minutes.
            2020019, //  Strawberry Candy - A very sweet, home-made strawberry candy. Magic Att. +5 for 30 minutes.
            2020020, //  Candy Basket - A basket full of sweet home-made fruit candies decorated with ribbons and marbles. Avoidability, speed and accuracy +10 each for 30 minutes.
            2020021, //  Chocolate - Milk chocolate that has a strong sweet scent. This is used to make the chocolate-dipped cookie stick.\nRecovers each of HP and MP by 1000.
            2020022, //  Corn - A fresh corn plucked right off the stalk.\nRecovers 100 MP.
            2020023, //  Roasted Turkey - A well-roasted turkey enough to feed the whole family.\nRecovers 100 HP.
            2020024, //  Coca_Cola - Sweet refreshing #cCoca-Cola#.\nRecovers HP and MP by 30%.
            2020025, //  Birthday Cake - A tasty-looking cake full of whipped cream and fruit toppings.\nRecovers 365 MP and HP.
            2020026, //  Pure Water - Very clean water.\nRecovers up to 800 MP.
            2020027, //  Red Bean Porridge - A hot steamy porridge made out of red beans. At an HP-decreasing map, whenever such map damage is dealt, 10 HP will be protected.
            2020028, //  Cider - A cold soft drink.\nIncreases weapon attack for 5 min.\nAccuracy -5 for 5 min., though.
            2020029, //  Unagi - Well-seasoned eel.\nRecovers 1000 HP.
            2020030, //  Song Pyun - Filled with royal jelly.\nRecovers 1500 HP.
            2020031, //  Han Gwa - A traditional Korean snack.\nRecovers 1500 MP.
            2020032, //  Rice-Cake Soup - Just got done boiling.\nRecovers 500 for both HP and MP.
            2022000, //  Triangular Sushi(plum) - A nice triangular sushi with plum in it. \nRecovers 20% of both HP and MP.
            2022001, //  Triangular Sushi(salmon) - A nice triangular sushi with salmon in it. \nRecovers 30% of both HP and MP.
            2022002, //  Triangular Sushi(bonito) - A nice triangular sushi with bonito in it. \nRecovers 40% of both HP and MP.
            2022003, //  Triangular Sushi(pollack) - A nice triangular sushi with pollack in it. \nRecovers 50% of both HP and MP.
            2022004, //  Triangular Sushi(mushroom) - A nice triangular sushi with mushroom in it. \nRecovers 60% of both HP and MP.
            2022005, //  Sushi(tuna) - Sushi made out of fresh fish near Victoria Island.\nRecovers 1000 HP.
            2022006, //  Sushi(salmon) - Sushi made out of fresh fish near Victoria Island.\nRecovers 500 HP.
            2022007, //  Dango - Taste the sweetness of this dango.\nRecovers 200 HP & MP.
            2022008, //  Mushroom Miso Ramen - Only the finest ingredients are used to make this Miso Ramen.\nRecovers 80% for both HP and  MP.
            2022009, //  Maple Special Bento - A special bento with meat and mushroom.\nRecovers 500 for both HP and MP.
            2022010, //  Ramen - A bowl of ramen cooked with Robo's special recipe.\nRecovers HP 1000.
            2022011, //  Kinoko Ramen(roasted pork) - A bowl of ramen cooked with roasted pork in the soup.\nRecovers HP 1500.
            2022012, //  Kinoko Ramen(pig head) - A bowl of ramen cooked with pig head in the soup.\nRecovers HP 800.
            2022013, //  Kinoko Ramen(salt) - A bowl of ramen cooked with salt in the soup. Tastes a little peculiar...\nRecovers HP 500.
            2022014, //  Fish Cake(skewer) - A Fish Cake skewer which also includes a bunch of vegetables.\nRecovers MP 250.
            2022015, //  Fish Cake(dish) - A dish full of tasty Fish Cake.\nRecovers MP 500.
            2022016, //  Tri-colored Dango - A tri-colored dango that includes a handful of tasty dango.\nRecovers each of HP and MP by 400.
            2022017, //  Takoyaki (Octopus Ball) - A hot, tasty-looking Takoyaki.nAttack +8 for 5 minutes.
            2022018, //  Takoyaki (jumbo) - Two servings worth of Takoyaki.nAttack +8 for 10 minutes.
            2022019, //  Yakisoba - A bowl of Yakisoba which includes vegetable, seafood, and noodles mixed with a delicious sauce.nMagic Attack +10 for 5 minutes.
            2022020, //  Yakisoba (x2) - Double the serving of a normal bowl of Yakisoba which includes vegetable, seafood, and noodles mixed with a delicious sauce.nMagic Attack +10 for 10 minutes.
            2022021, //  Valentine Chocolate (Dark) - A rich, dark chocolate for your special someone on Valentine's Day. Recovers 50% of HP and MP.
            2022022, //  Valentine Chocolate (Strawberry) - A rich, dark chocolate for your special someone on Valentine's Day. Recovers 100% of HP and MP.
            2022023, //  Valentine Chocolate (White) - A tasty white chocolate for your special someone on Valentine's Day. Accuracy +10 for 5 minutes.
            2022024, //  Cookie - Crispy on the cover and soft inside, this cookie is definitely worth a bite.
            2022025, //  Marshmallow - A mushy, yummy-looking marshmallow.
            2022026, //  Candy - A sparkling candy with a scent of tropical fruit.
            2022027, //  Zong Zi - A tiny seed of a fruit.\nRecovers 50 HP.
            2022028, //  Maple Cola - none
            2022029, //  Candy Basket - Defense and avoidablity will be increased by 300 for 30 minutes.
            2022030, //  Pink Rice Cake - Recovers 500 HP and MP.
            2022031, //  Rice Cookie - Recovers 1500 for both HP and MP.
            2022032, //  Nependeath's Honey - Recovers 1000 for both HP and MP.
            2022033, //  Air Bubble - Air bubble enables breathing in the water for 15 minutes.
            2022034, //  Fried shrimp - Recovers 500 of HP and MP.
            2022035, //  Cookie - Crispy on the outside, marshmellow-soft on the inside, this cookie can be traced from afar by its sweet smell.nWeapon & Magic Attack +20 for 30 minutes.
            2022036, //  Fruity Candy - Multi-colored, fruity-flavored candies.nSpeed +10 for 30 minutes.
            2022037, //  New Year Rice Cake - Attack +20 for 10 min.
            2022038, //  New Year Lunchbox - Recovers 2000 HP & MP
            2022039, //  Seaweed - null
            2022040, //  Cooked Sea Bream - Magic Attack +35 for 10 min.
            2022041, //  New Year Rice Soup - Accuracy +30 for 5 min.
            2022042, //  Steamed Crab - Weapon Def. +100 for 5 min.
            2022043, //  Roasted pork - A piece of roasted pork that is favorite of Yellow King Goblin. Marinated just right, and it even looks delicious enough for one to salivate over it. Recovers 800 HP.
            2022044, //  Buckwheat paste - A buckwheat paste that is the favorite of Green King Goblin. Bouncy like jelly, yet very nutritious. Recovers 800 MP.
            2022045, //  Rice Wine - A cup of wine made out of fermented rice that is the favorite of Blue King Goblin. An aroma of a combination of vinegar-like spike and smoothness of a tea tickles the nose of those near it. Recovers 400 each of HP and MP.
            2022046, //  Jujube - A ripe, red jujube.\nRecovers 30 HP.
            2022047, //  Pear - A big, juicy-looking pear.\nRecovers 30 HP.
            2022048, //  Persimmon - A ripe, orangy Persimmon.\nRecovers 30 HP.
            2022049, //  Chestnut - A ripe, brown chestnut just picked out of a tree.\nRecovers 30 HP.
            2022050, //  Tofu - Made out of soy beans, it is one healthy food recommended for everyone.\nRecovers 50 HP.
            2022051, //  Dumpling - A mixture of pork and vegetable wrapped up in a thin layer of wheat.\nRecovers 1500 HP.
            2022052, //  Lotus Perfume - A perfume that contains power-boosting aroma. nAttack, Defense, Magic Attack +10 for 20 min.
            2022053, //  Oriental Perfume - A perfume that contains power-boosting aroma. nAttack, Defense, Magic Attack +15 for 20 min.
            2022054, //  Chrysanthemum Perfume - A perfume that contains power-boosting aroma. nAttack, Defense, Magic Attack +20 for 20 min.
            2022055, //  Corn Stick - A roasted corn on a skewer. Very delicious looking. \nRecovers HP 800.
            2022056, //  Fruit Stick - A snackery with fruity-flavored candies on the skewer.\nRecovers MP 800.
            2022057, //  Yellow Easter Egg - A freshly boiled egg colored in yellow. Recovers 100 HP and MP.
            2022058, //  Green Easter Egg - A freshly boiled egg colored in green. \nRecovers 200 HP and MP.
            2022060, //  Yellow Cider - A cold soft-drink. Magic Attack +35 for 5 min.
            2022061, //  Red Cider - A cold soft-drink. Attack +34 for 5 min.
            2022062, //  Congrats from GM - A mystical spell that can only be casted by a GM as a sign of congratulation. Weapon & Magic Attack +20, Defense +100, Accuracy & Avoidability +50, Speed & Jump +10 for 1 HOUR.
            2022063, //  Korean Warrior - Weapon Attack +20, Magic Attack +20 for 10 min.
            2022064, //  Forza Corea - Gives +50 Weapon Defense and +50 Magic Defense for 10 minutes.
            2022065, //  A Prayer for Victory - Increases Jump +10, Speed +20 for 20 minutes.
            2022066, //  Oolleung Squid - Dried squid from Oolleung renowned for its taste. \nRecovers 500 HP & MP.
            2022068, //  Mini Coke - A sweet, tasty, carbonated Coke featured in a Mini Can. nAttack +8, Magic Attack +8 for 20 minutes.
            2022069, //  Coke Pill - A pill made out of a sweet, tasty, carbonated Coke. nAttack +10, Magic Attack +10, Defense +10 for 15 minutes.
            2022070, //  Coke Lite Pill - A pill made out of a sweet, tasty, carbonated Coke. nAttack +12, Magic Attack +12, Defense +12 for 15 minutes.
            2022071, //  Coke Zero Pill - A pill made out of a sweet, tasty, carbonated Coke. nAttack +15, Magic Attack +15, Defense +15 for 15 minutes.
            2022072, //  Barbecue - A fresh Barbecue meat.\nRecovers 1000 HP.
            2022073, //  Red Fruit - A curious-looking fruit that increases Attack by 8 for 10 minutes. Kicks in as soon as the first bite is taken.
            2022074, //  Black Fruit - A curious-looking fruit that increases Defense by 15 for 10 minutes. Kicks in as soon as the first bite is taken.
            2022075, //  Blue Fruit - A curious-looking fruit that increases Magic Attack by 10 for 10 minutes. Kicks in as soon as the first bite is taken.
            2022076, //  Baby Dragon Food - A delicious bowl of baby food for the baby dragon. Attack +7 for 20 minutes.
            2022077, //  Blessing from Wonky the Fairy - A blessing from Wonky the Fairy. Increases attack & magic attack.
            2022078, //  Blessing from Wonky the Fairy - A blessing from Wonky the Fairy. Increases weapon defense & magic defense.
            2022079, //  Blessing from Wonky the Fairy - A blessing from Wonky the Fairy. Increases accuracy and avoidability.
            2022086, //  Blessing from Wonky the Fairy - A blessing from Wonky the Fairy. Increases speed and jump.
            2022087, //  Chicken Soup - Weapon Attack+20, Magic Attack +30 for 15 minutes.
            2022088, //  Fried Chicken - Crispy on the outside, soft on the inside. \nRecovers HP 400.
            2022089, //  Chun Gwon - A dish full of renowned Chun Gwon. nnRecovers MP 400.
            2022090, //  Bubble Gum - A fruity bubble gum that can make a huge bubble.n Jump +5 for 20 minutes.
            2022091, //  HP up - HP up
            2022092, //  Song Pyun - Weapon Attack +20, Magic Attack +30 for 15 minutes.
            2022093, //  Han Gwa - Weapon Attack +20, Magic Attack +30 for 15 minutes.
            2022094, //  Massage Oil - A massage oil used for the Thai Body Massage session. Attack +8 for 10 minutes.
            2022096, //  Thai Cookie - A sweet, Thai treat. \nRecovers HP 150.
            2022097, //  Green Malady's Candy - A magical concoction bewitched for wellness by Malady. \nRecovers 50% of the HP. Also recovers 50% of MP.
            2022098, //  Red Malady's Candy - An enticing piece of candy straight from Malady's finest pot. \nRecovers around 300 MP.
            2022099, //  Blue Malady's Candy - This delicious candy holds a special blessing from Malady. \nRecovers all HP and MP.
            2022100, //  Horntail Squad : Victory - Weapon Attack +30, Magic Attack +40, Weapon Defense +200, Magic Defense +200 for one hour.
            2022101, //  The Breath of Nine Spirit - Weapon Attack +25, Magic Attack +35, Weapon Defense +150, Magic Defense +150 for one hour.
            2022102, //  Baby Witch - Weapon Attack +20, Magic Attack +30 nfor 15 minutes.
            2022103, //  Pumpkin Pie - A piping hot, delicious pie right from Grandma Benson's oven. Eat up! \nRecovers 700 HP, 400 MP and Defense +50 for 10 mins.
            2022105, //  Enchanted Apple Crisp - A hot pastry made from mystic apples. At an HP-decreasing map, whenever such map damage is dealt, 50% HP will be protected. (Modification Request)
            2022106, //  Peach - Recovers a set amount of HP and MP. Only obtainable in Mu Lung.
            2022107, //  Maple Syrup - A mystical syrup from maple tree. Weapon & Magic Attack +20, Defense +50, Accuracy & Avoidability +30, Speed & Jump +10 for 20 MINUTES.
            2022108, //  Admin's Congrats - A mysterious form of bless given by the administrator. Attack +10, Magic Attack +10, Defense+ 30, Accuracy +20, Avoidability +20, Speed +3 and Jump +3 for 20 minutes
            2022109, //  Tree Ornament - Weapon Attack +20, Magic Attack +30 forn15 minutes.
            2022112, //  Christmas Melon - A special melon imbued with Holiday blessings. Delicious! nRecovers 2000 HP and 1000 MP.
            2022113, //  Gelt Chocolate - A special piece of tasty chocolate given out at the Festival of Lights. \nRecovers 100 HP & MP, and +120 Attack +120 Weapon Def. +30 Accuracy +30 Avoidability + 10 Speed +10 Jump for 10 minutes.
            2022114, //  Banana Graham Pie - This scrumptious pie is sure to lighten your spirits! nRecovers 400 HP & 500 MP, and +120 Magic +120 Magic Def. +30 Accuracy +30 Avoidability + 10 Speed +10 Jump for 10 minutes.
            2022116, //  Magic of Kasandra - Double the meso drop rate.
            2022117, //  Increase in Weapon Defense - An increase in weapon defense with a little help from a Dark Spirit.
            2022118, //  Increase in Magic Defense - An increase in magic defense with a little help from a Dark Spirit.
            2022119, //  Increase in Accuracy - An increase in accuracy with a little help from a Dark Spirit.
            2022120, //  Increase in Avoidablility - An increase in avoidability with a little help from a Dark Spirit.
            2022121, //  Increase in Attack - An increase in attack with a little help from a Dark Spirit.
            2022123, //  Blossom Juice - A special energy drink made from crushed Cherry Blossoms and other mystic ingredients. Recovers 1200 HP, 900 MP and +12 to DEF for 20 minutes.
            2022124, //  Ginseng Concentrate - A ginseng concentrate. Recovers both HP and MP by 400.
            2022125, //  Bellflower Concentrate - A Bellflower concentrate. Recovers both HP and MP by 200.
            2022126, //  Mind & Heart Medicine - A hot, soup-like medicine made out of bear paws. Drinking this will revitalize the brain to the tune of Accuracy +10 for 15 minutes.
            2022127, //  Mastery Medicine - A hot, soup-like medicine that allegedly turns students into bona-fide Masters. Magic Attack +10 for 15 minutes.
            2022128, //  Body & Physics Medicine - A hot, soup-like medicine made out of snake tails. Drinking this will revitalize the body to the tune of Attack +8 for 15 minutes.
            2022129, //  Canned Peach - Homemade canned peach made by the weird pharmacist in Mu Lung, Dr. Do.\nRecovers around 1000 nHP.
            2022130, //  Peach Juice - Peace juice made by the weird pharmacist in Mu Lung, Dr. DonRecovers around 500 MP.
            2022131, //  Bellflower Medicine Soup - A herb medicine made from bellflowers and snake.nAttack + 6 for 10 minutes.
            2022132, //  Pill of Tunnel Vision - A pill of acorn powder.  The marksman's liquid medicine.  Increases +12 accuracy for 10 minutes.
            2022142, //  Pill of Intelligence - A pill of powdered deer antler and wild ginseng.  Increases Magic Attack by +10 for 10 minutes.
            2022143, //  Slithering Balm - Ointment medicine made from Mr. Alli's skin.  Gives +12 Avoidability for 10 minutes.
            2022144, //  Cassandra's Magic - A mysterious spell cast by Cassandra.  Gives +20 Att and Magic, +100 Defense, +50 accuracy and avoidability, and +10 speed and jump for 1 hour.
            2022145, //  Cassandra's Magic - A mysterious spell cast by Cassandra.  Gives +10 Att and Magic, +30 Defense, +20 accuracy and avoidability, and +3 speed and jump for 20 minutes.
            2022146, //  Happy birthday - Gives +20 W. Att, +30 M. Att for 15 minutes to all the characters in the map when item was used.
            2022147, //  Petit Rose - Gives +20 Weapon Attack, +30 Magic Attack for 15 minutes.
            2022148, //  Desert Mist - Pure water extracted from Katuse roots.  Recovers 200 MP.
            2022149, //  Black Bean Noodle - Nothing like a hot bowl of noodles to cap off a great day. Increases Weapon Attack and Magic Attack by +13 for 30 minutes.
            2022150, //  Party Mana Elixir - Recovers 300 MP for all members of your party.
            2022151, //  Party Elixir - Recovers 50% of HP and MP for all members of your party.
            2022152, //  Party Power Elixir - Recovers all HP and HP for all members of your party.
            2022153, //  Party All Cure Potion - Cures any abnormal state affecting any members of your party.
            2022154, //  Mini Cube of Darkness - One of the members in the opposing party will have their buffs nullified.
            2022155, //  Cube of Darkness - Everyone in the opposing party will have their buffs nullified.
            2022156, //  Stunner - One of the members of the opposing party will be stunned.
            2022157, //  White Potion - A highly-concentrated potion made out of red herbs.\nRecovers 300 HP.
            2022158, //  Onyx Apple - A rare, ripe apple imbued with power. Recovers 90% HP & MP, and provides +100 W.Att, +100 M.Att, +20 Def for 10 minutes.
            2022159, //  Amorian Rice Cookie - A special Rice Cookie from Amos. Recovers 3000 HP and MP.
            2022160, //  Victoria's Amorian Basket - Victoria's special basket, made just for Amos. Provides a boost of +40 to Avoidability, speed and accuracy for 10 minutes
            2022161, //  Crystalized Pineapple Chew - A very sweet, home-made pineapple chew. Provides a bonus of +20 ATT for 7 minutes.
            2022162, //  Flower Shower - Increases physical attack by 20, Magic attack by 30 for 15 minutes.
            2022163, //  Maple BBQ - Delicious, succulent BBQ meat. Take a big bite--you know you want to.  Weapon attack +13, Magic Attack +13 for 20 min.
            2022164, //  Fireworks - The fireworks for celebrating MV's defeat. Attack +20, Magic Attack +20 for 10 min.
            2022165, //  Soft White Bun - Freshly baked bread, hot from the oven!  Makes you warm inside and prevents HP loss for 30 minutes in the El Nath region.
            2022166, //  Cassandra's Magic - Breathe comfortably underwater for 30 minutes with Cassandra's magic.  Prevents the constant HP damage you receive for every 10 seconds underwater.
            2022174, //  Grilled Cheese - A tasty, golden brown, grilled cheese sandwich. Recovers 500 HP & MP and gives a boost of +20 Def for 30 mins.
            2022175, //  Cherry Pie - A piping hot pie that warrants eating. Recovers 2000 HP/MP and has a bonus of +2 ATT for 8 minutes.
            2022176, //  Supreme Pizza - A piping hot pizza with all toppings. Recovers 900 HP and 600 MP.
            2022177, //  Waffle - A hot, buttery waffle that's ready to eat. Recovers 300 HP and MP
            2022178, //  Merlin Orb - A majestic orb said to have been the property of a powerful mage. Gives a boost of +40 M. Att for 15 mins.
            2022179, //  Leaf Crystal - An uncommon crystal formed by the leaves of an Ellinian vine. Provides a boost of +12 W. Att, +20 M. Att, +20 DEF/M. DEF, +8 Avoidability/Accuracy for 20 minutes. May only be used 5 times before it vanishes.
            2022180, //  Mapleade - Refreshing drink for thirsty travelers! Recovers 80% HP, 90% MP and gives a boost of +2 Att for 30 mins.
            2022181, //  Wedding Bouquet - A special wedding bouquet.  Gives +5 Weapon Attack and +3 Magic Attack for 5 minutes.
            2022182, //  Wedding Bouquet - A special wedding bouquet.  Gives +8 Weapon Attack and +5 Magic Attack for 5 minutes.
            2022183, //  Russellon's Pills - A pill made by Russellon from Magatia.  The actual effects of this pill remain a mystery...
            2022184, //  Russellon's Potion - A potion made by Russellon from Magatia. The actual effects of this potion remain a mystery...
            2022185, //  Wedding Bouquet - A special wedding bouquet.  Gives +10 Weapon Attack and +8 Magic Attack for 5 minutes.
            2022186, //  Laksa - Singapore local speciality spicy noodle. It triggers one to sweat after consumpsion.\nRecovers 800 HP.
            2022187, //  Hokkien Mee - Singapore's local speciality prawn noodles. It is normally served with prawns, chili & lime. \nRecovers 1200 HP.
            2022189, //  Carrot Cake - A delicious local traditional food. It is made up of carrot extracts & secret recipes. \nRecovers 1800 HP.
            2022190, //  Chicken Rice - A delicious Singapore local traditional food. The rice is covered with the fragrant secret recipes of chicken extracts.\nRecovers 2200 HP.
            2022191, //  Satay - A delicious Singapore local traditional BBQ food. It is normally served with curry sauces. Its fragrance can be sensed a distance away.\nRecovers 2600 HP.
            2022192, //  Guava - A fruit that is hard & sour. Its nutritional value improves our digestive system.\nRecovers 500 MP.
            2022193, //  Rambutan - A fruit that is delicious & juicy. It is so sweet that it attracts ants if they are not well attended to. \nRecovers 800 MP
            2022194, //  Dragon Fruit - Another fruit that is delicious & juicy, similar to Rambutan, it will attract unwanted pests if they are not attended. \nRecovers 1600 MP.
            2022195, //  Durian - The fruit is nominated as the King of Fruits around the region. Despite having a hard & thorny shell, the fruit that lies within is extremely fragrant, and it tastes great! nRecovers 3200 MP.
            2022196, //  Nasi Lemak - A popular Malay Traditional Dish mainly made up of rice, egg & cucumber, soaked with pandan fragrant.nImproves Magic Attack +8 for 5 minutes.
            2022197, //  Roti Prata - A popular Indian Traditional Dish made up of flour. It is normally served with sugar & curry sauces.n Improves Magic Attack +8 for 10 Minutes.
            2022198, //  Pepper Crab - A popular Chinese traditional dish made of up fresh steam crab, pepper, egg & secret recipes.nImproves Weapon Attack +8 for 5 minutes.
            2022199, //  Chili Crab - A popular Chinese traditional dish made of up fresh steam crab, chili power, egg & secret recipes.nImproves Weapon Attack +8 for 10 minutes.
            2022200, //  Russellon's Potion - A potion made by Russellon from Magatia. The actual effects of this potion remain a mystery...
            2022203, //  Russellon's Potion - A potion made by Russellon from Magatia. The actual effects of this potion remain a mystery...
            2022204, //  Russellon's Potion - A potion made by Russellon from Magatia. The actual effects of this potion remain a mystery...
            2022205, //  Russellon's Potion - A potion made by Russellon from Magatia. The actual effects of this potion remain a mystery...
            2022206, //  Russellon's Potion - A potion made by Russellon from Magatia. The actual effects of this potion remain a mystery...
            2022207, //  MesoGears Ring - An ancient ring of wondrous power. There appears to be a faded inscription along the side..."Subani". Provides a boost of +8 W. Att, +15 M. Att, +12 Def/M. Def. for 8 minutes.
            2022208, //  Cassandra's Magic - A special magic spell cast by Cassandra. For 30 minutes, Attack +10, Magic Attack +10, DEF +30, Accuracy +20, Avoidability +20, Speed +7, and Jump +5.
            2022209, //  Cassandra's Magic - A special magic spell cast by Cassandra. For 1 hour, Attack +10, Magic Attack +10, DEF +30, Accuracy +20, Avoidability +20, Speed +7, and Jump +5.
            2022210, //  Edmund's Special Brew - A healing tonic made with Edmund's secret recipe.  Just the thing when you're feeling under the weather!  [Restores 50% of HP and MP, also gives +14 W. Att + 30 M.Att, for 10 minutes.]
            2022211, //  Sophilia's Necklace - A jewel crafted by Prendergast for his daughter, intended as a protective gift for her 16th birthday.
            2022212, //  Smore - A tasty, hot smore. Perfect for a toasty Halloween night!
            2022213, //  Heartstopper - Just one taste of this spicy candy and it'll feel like your heart's on fire!
            2022214, //  Pumpkin Taffy - Sweetened pumpkin taffy on a candy cane stick.  [Gives +15 Weapon Defense, +15 Magic Defense for 5 minutes]
            2022215, //  Red Gummy Slime - Super-chewy gummy slimes. This one is cherry-flavored.  If only real Slimes tasted this good. [Restore 1200HP, 1200MP]
            2022224, //  Green Gummy Slime - Super-chewy gummy slimes. This one is lime-flavored.  If only real Slimes tasted this good. [Restores 600 HP]
            2022225, //  Purple Gummy Slime - Super-chewy gummy slimes. This one is grape-flavored.  If only real Slimes tasted this good. [Restores 600 MP]
            2022226, //  Orange Gummy Slime - Super-chewy gummy slimes. This one is orange-flavored.  If only real Slimes tasted this good. [Restores 600HP, 600MP]
            2022227, //  Maple Pop - A mouth-watering, delectable sweet treat! [Gives +100 Accuracy for 1 minute]
            2022228, //  Tae Roon's Note - A small note written by Tae Roon. This note is enchanted, so it boosts weapon attack and magic attack by 3 for 1 minute.
            2022238, //  Mushroom Candy - Delicious mushroom candy from Sen. This candy makes anyone feel good about themselves. So good, that it boosts Jump +3 for 3 minutes.
            2022239, //  Pumpkin Pieces - Pieces of pumpkin left over from making halloween Jack-o'-Lanterns. The pumpkin, perfectly ripe, has an aroma that is sweeter than ever. Each piece recovers 50 HP and 50 MP.
            2022240, //  Halloween Candy - A candy wrapped in stripes. Kids love it. Recovers 20 HP and MP.
            2022242, //  Power of the Glowing Rock - Received a mysterious power from the Glowing Rock. Boosts the weapon attack and magic attack slightly for 10 minutes.
            2022243, //  Coconut Juice - A small hole is up on the top of the Coconut, where the straw goes in. Drink the juice with the straw for maximum refreshment. Recovers 100 HP.
            2022244, //  Attack Crystal - A red crystal with a mysterious power packed in. Attack +5 for 5 minutes.
            2022245, //  Accuracy Crystal - A blue crystal with a mysterious power packed in. Accuracy +5 for 5 minutes.
            2022246, //  Stuffing Scoop - A delicious scoop of stuffing. This treat comes around only once in a while so eat it up before it gets cold. \n[Restores 600 HP/ 600 MP]
            2022247, //  Cranberry Sauce - This delicious cranberry sauce complements almost any meal! \n[Gives + 30 Magic Attack for 3 minutes]
            2022248, //  Mashed Potato - It looks like someone dropped this potato. \n[Restores 800 HP]
            2022249, //  Gravy - The only thing better than a gravy boat is a gravy train. \n[Restores 800 MP]
            2022250, //  Snowing Fishbread - Increases Physical Attack Power by 20 and Magic Attack Power by 30 for 15 minutes.
            2022251, //  Power Punch - A fist used during the Hunting Tournament. Boosts weapon and magic attack.
            2022252, //  Wing of the Wind - A set of wings used during the Hunting Tournament. Boosts speed and jump.
            2022253, //  Crazy Skull - An eyeball-rotating skull used during the Hunting Tournament. Inverts directions.
            2022255, //  Shield - A shield used during the Hunting Tournament. Protects the owner from the bomb once.
            2022256, //  Maplemas Ham - A delicious looking Christmas Ham with a sprig of mistletoe on top.\nRecovers 3000 HP and MP.
            2022257, //  Smoken Salmon - The traditional Versalmas dinner.  Smells... like fish. \nRecovers 2385 HP and 3791 MP.
            2022258, //  Ssiws Cheese - Cheese from the alternate dimension of Versal.  Looks funny but smells quite nice.  nGives +220 Magic Attack for 2 minutes
            2022259, //  Sugar-Coated Olives - Olives frosted with pink sugar -- a special treat beloved by children from Versal! nGives +40 Speed and + 25 Jump for 5 minutes.
            2022260, //  Caramel Onion - O-Pongo's favorite treat!  Creamy caramel with a crunchy onion center! nRestores 800 HP and MP.
            2022261, //  Chocolate Wafers - A crispy wafer layered with chocolate creme. nGives +40 Weapon Attack for 3 minutes
            2022262, //  Sunblock - SPF 1000.  Blocks all harmful rays, including magical ones. nGives +200 Magic Defense for 10 minutes.
            2022263, //  Lump of Coal - Restores 1 HP and 1 MP.
            2022264, //  Snow Cake Piece - A piece of cake that consists of snow-white whipped cream and cherry. Recovers 300 HP and MP.
            2022265, //  A Flurry of Snow - Increases Physical Attack Power by 20 and Magic Attack Power by 30 for 15 minutes.
            2022266, //  Chinese Firecrackers - A string of Chinese firecrackers known to scare away ghoulish spirits.
            2022267, //  Naricain's Demon Elixir - A fiery black liquid that gives the user the power of a thousand demons when consumed. [Gives +140 Weapon Attack for 8 minutes]
            2022268, //  Subani's Mystic Cauldron - Drinking the swirling blue liquid within this small iron pot fills the user with an energy that emanates a protective aura. [Gives +100 Overall Defense, +200 Magic Attack for 10 minutes]
            2022269, //  Barricade Booster - John Barricade's special concoction, used to get him out of tight jams.  For use in emergencies!  [Gives +50 Avoidability, +50 Accuracy, +10 Jump for 5 minutes]
            2022271, //  Sweet Heart - Weapon attack +20, Magic attack +30 for 15 minutes.
            2022272, //  Power Scream - A power scream from Maple Admin. Weapon attack +8, Magic attack +12 for 30 minutes.
            2022273, //  Party Bear - Weapon attack +20, Magic attack +30 for 15 minutes.
            2022274, //  Taru Face Paint - This mystical camouflage allows the user to blend into his or her surroundings. [Gives +100 Avoidability for 5 minutes.]
            2022275, //  Primal Brew - A mixture made from an ancient Taru shaman recipe.  Instills warriors with the primal strength of the Jungle Spirit. [Gives +35 Weapon Attack, +10 Accuracy for 20 minutes.]
            2022276, //  Spirit Herbs - Special incense used by ancient Taru shamans for communion rituals with the Jungle Spirit. [Gives +90 Magic Attack for 20 minutes.]
            2022277, //  Jungle Juice - A delicious natural beverage made from a secret blend of jungle fruits, flowers, roots, and vines. The perfect thing to fuel a Taru brave through a long jungle trek!  [Restores 1000 HP and 2000 MP.]
            2022278, //  Treasure Hunt Note - Spread all over the Maple World for the 4th anniversary of MapleStory, this note contains the prizes that you'll win when this is found.
            2022279, //  Chocolate Cream Cupcake - A delicious chocolate cupcake with vanilla cream filling. One taste and you'll be hooked!  [Restores 300 HP & MP, and gives +30 Accuracy, +30 Speed, and +30 Jump for 3 minutes.]
            2022280, //  Big Cream Puff - A rare, tasty dessert. Known as the 'Warrior's Dessert' to some and the 'Fattening Treat' to others. [Gives +30 Weapon Attack for 3 minutes.]
            2022281, //  Agent O's Encouragement - An encouraging message from Agent O.  Jump rate will be increased by 20 for 30 minutes.
            2022282, //  Agent O's Encouragement - An encouraging message from Agent O.  Speed will be increased by 40 for 30 minutes.
            2022283, //  Baby Chick Cookie - This is a cookie shaped like a baby chick. Recovers 1000 HP and MP.
            2022284, //  Secret Box - Secret Box
            2022285, //  Sorcerer's Potion - This is a potion that you can buy from Sorcerer. It's potent, but its side-effects are equally strong. It's also very expensive, so be careful with it.
            2022296, //  VitroJuice - A futuristic power pack full of liquid fuel synthesized by T-1337.  Looks suspiciously like an energy drink.  Drink at your own risk! [Gives +14 Weapon Attack for 15 minutes]
            2022302, //  NitroJuice - A futuristic power pack full of liquid fuel synthesized by T-1337.  Looks suspiciously like an energy drink.  Drink at your own risk! [Gives +22 Weapon Attack for 10 minutes]
            2022305, //  BlastroJuice - A futuristic power pack full of liquid fuel synthesized by T-1337.  Looks suspiciously like an energy drink.  Drink at your own risk! [Gives +90 Weapon Attack for 1 minute]
            2022306, //  ElectroJuice - A futuristic power pack full of liquid fuel synthesized by T-1337.  Looks suspiciously like an energy drink, but drink at your own risk! [Gives +50 Magic Attack for 10 minutes.]
            2022307, //  MegaJuice - A futuristic power pack full of liquid fuel synthesized by T-1337.  Looks suspiciously like an energy drink, but drink at your own risk! [Gives +200 Magic Attack for 30 seconds.]
            2022308, //  GigaJuice - A futuristic power pack full of liquid fuel synthesized by T-1337.  Tasty and strong enough to dissolve rust from machinery!  Drink at your own risk! [Gives +700 Magic Attack for 10 seconds.]
            2022309, //  JigaJuice - A futuristic power pack full of liquid fuel synthesized by T-1337.  Drink at your own risk!   Gives the user a sudden jolting surge of energy, so use it or lose it! [Gives +1000 Magic Attack for 5 seconds.]
            2022310, //  The Energizer Drink - An energizing drink packed with electrolytes. For 30 minutes, you will receive a boost of: Attack +25, Magic Attack +30, Defense +30
            2022311, //  Tick-Tock's Egg - This is Tick-Tock's egg. There must be something inside.
            2022323, //  Cronos' Egg - This is Cronos' egg. There must be something inside.
            2022324, //  Holiday Buff - A Holiday present from the Snow Spirit. For 15 min., Speed +5.
            2022326, //  Holiday Buff - A Holiday present from the Snow Spirit. For 15 min., Jump +7.
            2022332, //  Holiday Buff - A Holiday present from the Snow Spirit. For 15 min., Speed and Jump +10.
            2022333, //  Elixir of Darkness - A mysterious concoction of herbs brewed deep within the mountains of El Nath. [Gives +200 Magic Attack, -25 Defense for 5 minutes.]
            2022335, //  Gold Dust - Ancient dust found long ago by the miners in El Nath. [Gives +20 Defense for 5 minutes.]
            2022336, //  Adonis Cauldron - A rather clumsy attempt at potion creation by Adonis. Still useful in the hands of a skilled warrior. [Gives +40 Weapon Attack, +50 Avoidability, -30 Defense for 10 minutes.]
            2022337, //  Fireworks - The fireworks for celebrating MV's defeat. Speed +5, Physical & Magic Attack +5 for 20 min.
            2022338, //  Mihile's Blessing - Once I used the 'Torn Cygnus' Book', a mysterious power covered me and blessed me.
            2022339, //  Oz's Blessing - Once I used the 'Torn Cygnus' Book', a mysterious power covered me and blessed me.
            2022340, //  Irena's Blessing - Once I used the 'Torn Cygnus' Book', a mysterious power covered me and blessed me.
            2022341, //  Eckhart's Blessing - Once I used the 'Torn Cygnus' Book', a mysterious power covered me and blessed me.
            2022342, //  Hawkeye's Blessing - Once I used the 'Torn Cygnus' Book', a mysterious power covered me and blessed me.
            2022343, //  Pink Bean Squad : Victory - Weapon Attack +35, Magic Attack +45, Weapon Defense +250, Magic Defense +250 for one hour.
            2022344, //  Fireworks - The fireworks for celebrating MV's defeat. Speed +5, Physical & Magic Attack +5 for 20 min.
            2022345, //  Cygnus's Blessing - Once I completed Cygnus' Book, the spirit's power covered me and blessed me.  It increased my Attack Rate by 10, Physical Defense Rate by 80 and  Speed by 5 for 10 minutes.
            2022352, //  Red Easter Egg - A freshly boiled egg colored in red. Recovers 400 HP and MP.
            2022354, //  Return Scroll - Nearest Town - Returns you to the nearest town.
            2022355, //  Return Scroll to Lith Harbor - Returns you to Lith Harbor.
            2022356, //  Return Scroll to Ellinia - Returns you to Ellinia.
            2022357, //  Return Scroll to Perion - Returns you to Perion.
            2022358, //  Return Scroll to Henesys - Returns you to Henesys.
            2022359, //  Return Scroll to Kerning City - Returns you to the dark Kerning City.
            2022360, //  Return Scroll to Sleepywood - Returns you to Sleepywood, a quiet and dark forest-town.
            2022361, //  Return Scroll for Dead Mine - Returns you to the dead mine at the higher ground of El Nath.nCan only be used in Orbis and El Nath.
            2022362, //  Coffee Milk - Returns you to the nearest town.
            2022363, //  Strawberry Milk - Returns you to Mushroom Shrine.
            2022364, //  Fruit Milk - Returns you to Showa Town.
            2022365, //  Command Center Warp Capsule - A warp capsule that allows the owner of the capsule to warp to the Command Center of Omega Sector.
            2022366, //  Ludibrium Warp Capsule - A warp capsule that returns you to Ludibrium.
            2022367, //  Phyllia's Warp Powder - Warp powder made by fairy Phyllia.  Teleports you to Magatia when used inside the Nihal desert region.
            2022368, //  Nautilus Return Scroll - This scroll enables you to return to the Pirate village, Nautilus. This is a one use item and will disappear after use.
            2022369, //  Return to New Leaf City Scroll - Use this scroll to venture back to New Leaf City whenever you want!
            2022370, //  Masked Man's Invitation - An invitation from the Masked Man to the Halloween Party at the Haunted Mansion. Double-click to move straight to the mansion.
            2022371, //  Studio Invitation - An invitation to the studio for the event "For Guild Only".
            2022372, //  Scroll for Helmet for DEF - Improves the helmet's weapon def.\nSuccess rate:100%, weapon def. +1
            2022373, //  Scroll for Helmet for DEF - Improves helmet def.\nSuccess rate:60%, weapon def.+2, magic def., +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022374, //  Scroll for Helmet for DEF - Improves helmet def.\nSuccess Rate:10%, weapon def.+5, magic def.+3, accuracy+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022375, //  Scroll for Helmet for HP - Improves MaxHP on hats.\nSuccess rate:100%, MaxHP+5
            2022376, //  Scroll for Helmet for HP - Improves MaxHP on hats.\nSuccess rate:60%, MaxHP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2022377, //  Scroll for Helmet for HP - Improves MaxHP on hats.\nSuccess rate:10%, MaxHP+30. The success rate of this scroll can be enhanced by Vega's Spell.
            2022378, //  Scroll for Helmet for DEF - Improves helmet def.\nSuccess rate:100%, weapon def.+5, magic def.+3, accuracy+1
            2022379, //  Scroll for Helmet for HP - Improves MaxHP on hats.\nSuccess rate:100%, MaxHP+30
            2022380, //  Dark scroll for Helmet for DEF - Improves helmet def.\nSuccess rate:70%, weapon def.+2, magic def.+1nIf failed, the item will be destroyed at a 50% rate.
            2022381, //  Dark Scroll for Helmet for DEF - Improves the helmet def.\nSuccess rate:30%, weapon def.+5, magic def.+3, accuracy+1nIf failed, the item will be destroyed in a 50% rate.
            2022382, //  Scroll for Helmet for HP - Improves MaxHP on hats.\nSuccess Rate:70%, MaxHP+10nIf failed, the item will be destroyed in a 50% rate.
            2022383, //  Dark Scroll for Helmet for HP - Improves MaxHP on hats.\nSuccess Rate:30%, MaxHP+30nIf failed, the item will be destroyed in a 50% rate.
            2022384, //  Dark Scroll for Helmet for INT - Improves INT on hats.\nSuccess Rate: 70%. INT+2nIf failed, the item will be destroyed in a 50% rate.
            2022385, //  Dark Scroll for Helmet for INT - Improves INT on hats.\nSuccess Rate: 30%, INT +3nIf failed, the item will be destroyed in a 50% rate.
            2022386, //  Dark Scroll for Helmet for Accuracy - Improves the accuracy on the helmet.\nSuccess Rate 70%, Dex+1, accuracy +2nIf failed, the item will be destroyed at a 50% rate.
            2022387, //  Dark Scroll for Helmet for Accuracy - Improves the accuracy on the helmet.\nSuccess Rate 30%, Dex+2, accuracy +4nIf failed, the item will be destroyed at a 50% rate.
            2022388, //  Scroll for Helmet for Accuracy - Improves the helmet's accuracy option.\nSuccess Rate 10%, Dex+2, Accuracy +4. The success rate of this scroll can be enhanced by Vega's Spell.
            2022389, //  Scroll for Helmet for Accuracy - Improves the helmet's accuracy option.\nSuccess Rate 60%, Dex+1, Accuracy +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022390, //  Scroll for Helmet for Accuracy - Improves the helmet's accuracy option.\nSuccess Rate 100%, Accuracy +1
            2022391, //  Scroll for Helmet for DEF - Improves Weapon Defense on a Helmet.\nSuccess rate: 65%, Weapon Def. +2, Magic Def. +1
            2022392, //  Scroll for Helmet for DEF - Improves Weapon Defense on a Helmet.\nSuccess rate: 15%, Weapon Def.+5, Magic Def.+3, Accuracy+1
            2022393, //  Scroll for Helmet for MaxHP - Improves MaxHP on a Helmet.\nSuccess rate: 65%, MaxHP +10
            2022394, //  Scroll for Helmet for MaxHP - Improves MaxHP on a Helmet.\nSuccess rate: 15%, MaxHP +30
            2022395, //  Scroll for Rudolph's Horn 60% - Increases the weapon attack and magic attack of Rudolph's Horn.\nSuccess rate:60%, attack +1, magic att. +1
            2022396, //  Scroll for Helmet for INT 100% - Improves INT on headwear..Success rate 100%, INT+1
            2022397, //  Scroll for Helmet for INT 60% - Improves INT on headwear.\nSuccess rate 60%, INT+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022398, //  Scroll for Helmet for INT 10% - Improves INT on headwear.\nSuccess rate 10%, INT+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022399, //  Scroll for Helmet for DEX 100% - Improves DEX on headwear..Success rate 100%, DEX+1
            2022400, //  Scroll for Helmet for DEX 70% - Improves DEX on headwear..Success rate 70%, DEX+2nIf failed, the item will be destroyed at a 50% rate.
            2022401, //  Scroll for Helmet for DEX 60% - Improves DEX on headwear.\nSuccess rate 60%, DEX+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022402, //  Scroll for Helmet for DEX 30% - Improves DEX on headwear..Success rate 30%, DEX+3nIf failed, the item will be destroyed at a 50% rate.
            2022403, //  Scroll for Helmet for DEX 10% - Improves DEX on headwear.\nSuccess rate 10%, DEX+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022404, //  Scroll for Face Accessory for HP - Improves MaxHP on face accessories.\nSuccess rate:10%, MaxHP +30. The success rate of this scroll can be enhanced by Vega's Spell.
            2022405, //  Scroll for Face Accessory for HP - Improves MaxHP on face accessories.\nSuccess rate:60%, MaxHP +15. The success rate of this scroll can be enhanced by Vega's Spell.
            2022406, //  Scroll for Face Accessory for HP - Improves MaxHP on face accessories.\nSuccess rate:100%, MaxHP +5
            2022407, //  Dark Scroll for Face Accessory for HP - Improves MaxHP on face accessories.\nSuccess rate:30%, MaxHP +30 nIf failed, the item will be destroyed at a 50% rate.
            2022408, //  Dark Scroll for Face Accessory for HP - Improves MaxHP on face accessories.\nSuccess rate:70%, MaxHP +15 nIf failed, the item will be destroyed at a 50% rate.
            2022409, //  Scroll for Face Accessory for Avoidability - Improves avoidability on face accessories.\nSuccess rate:10%, Avoidability +2, DEX +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022410, //  Scroll for Face Accessory for Avoidability - Improves avoidability on face accessories.\nSuccess rate:60%, Avoidability +1, DEX +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022411, //  Scroll for Face Accessory for Avoidability - Improves avoidability on face accessories.\nSuccess rate:100%, Avoidability +1
            2022412, //  Dark Scroll for Face Accessory for Avoidability - Improves avoidability on face accessories.\nSuccess rate:30%, Avoidability +2, DEX +2 nIf failed, the item will be destroyed at a 50% rate.
            2022413, //  Dark Scroll for Face Accessory for Avoidability - Improves avoidability on face accessories.\nSuccess rate:70%, Avoidability +1, DEX +1 nIf failed, the item will be destroyed at a 50% rate.
            2022414, //  Scroll for Eye Accessory for Accuracy - Improves accuracy on eye accessories.\nSuccess rate:10%, Accuracy +3, DEX +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022415, //  Scroll for Eye Accessory for Accuracy - Improves accuracy on eye accessories.\nSuccess rate:60%, Accuracy +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022416, //  Scroll for Eye Accessory for Accuracy - Improves accuracy on eye accessories.\nSuccess rate:100%, Accuracy +1
            2022417, //  Dark Scroll for Eye Accessory for Accuracy - Improves accuracy on eye accessories.\nSuccess rate:30%, Accuracy +3, DEX +1 nIf failed, the item will be destroyed at a 50% rate.
            2022418, //  Dark Scroll for Eye Accessory for Accuracy - Improves accuracy on eye accessories.\nSuccess rate:70%, Accuracy +2 nIf failed, the item will be destroyed at a 50% rate.
            2022419, //  Scroll for Eye Accessory for INT - Improves INT on eye accessories.\nSuccess rate:10%, INT +3, Magic Def. +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022420, //  Scroll for Eye Accessory for INT - Improves INT on eye accessories.\nSuccess rate:60%, INT +1, Magic Def. +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022421, //  Scroll for Eye Accessory for INT - Improves INT on eye accessories.\nSuccess rate:100%, INT +1
            2022422, //  Dark Scroll for Eye Accessory for INT - Improves INT on eye accessories.\nSuccess rate:30%, INT +3, Magic Def. +2 nIf failed, the item will be destroyed at a 50% rate.
            2022423, //  Dark Scroll for Eye Accessory for INT - Improves INT on eye accessories.\nSuccess rate:70%, INT +1, Magic Def. +1 nIf failed, the item will be destroyed at a 50% rate.
            2022424, //  Scroll for Earring for INT - Improves INT on ear accessory.\nSuccess rate:100%, magic attack+1
            2022428, //  Scroll for Earring for INT - Improves INT on ear accessory.\nSuccess rate:60%, magic attack +2, INT+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022429, //  Scroll for Earring for INT - Improves INT on ear accessory.\nSuccess rate:10%, magic attack +5, INT+3, magic def. +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022430, //  Scroll for Earring for INT - Improves INT on ear accessory.\nSuccess rate:30%, magic attack +5, INT+3, magic def. +1
            2022431, //  Dark scroll for Earring for INT - Improves INT on ear accessory.\nSuccess rate:70%, magic attack +2, INT+1nIf failed, the item will be destroyed at a 50% rate.
            2022432, //  Dark scroll for Earring for INT - Improves INT on ear accessory.\nSuccess rate:30%, magic attack +5, INT+3, magic def. +1nIf failed, the item will be destroyed at a 50% rate.
            2022433, //  Dark scroll for Earring for DEX - Improves DEX on ear accesrroy.\nSuccess rate: 70%. DEX + 2nIf failed, the item will be destroyed at a 50% rate.
            2022434, //  Dark scroll for Earring for DEX - Improves DEX on ear accessorynSuccess rate: 30%. DEX + 3nIf failed, the item will be destroyed at a 50% rate.
            2022436, //  Dark Scroll for Earrings for DEF - Improves DEF on earringsnSuccess Rate 70%, weapon defense+1, magic defense+1nIf failed, the item will be destroyed at a 50% rate.
            2022437, //  Dark Scroll for Earrings for DEF - Improves DEF on earringsnSuccess Rate 30%, weapon defense+3, magic defense+3, accuracy+1nIf failed, the item will be destroyed at a 50% rate.
            2022438, //  Scroll for Earring for DEF - Improves DEF on earrings.\nSuccess Rate 10%, weapon defense+3, magic defense+3, Accuracy+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022439, //  Scroll for Earring for DEF - Improves DEF on earrings.\nSuccess Rate 60%, weapon defense+1, magic defense+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022440, //  Scroll for Earring for DEF - Improves DEF on earringsnSuccess Rate 100%, weapon defense+1
            2022441, //  Scroll for Earring for INT - Improves INT on Earrings.\nSuccess rate: 65%, Magic Attack +2, INT+1
            2022442, //  Scroll for Earring for INT - Improves INT on Earrings.\nSuccess rate:15%, Magic Attack +5, INT +3, Magic Def. +1
            2022443, //  [4yrAnniv]Scroll for Earring for INT - Improves INT on Maple Earring.\nSuccess rate: 40%, Magic Attack +3, INT +2 nIf failed, the item has a 30% chance of being destroyed.
            2022444, //  Scroll for Earring for DEX 100% - Improves DEX on earrings..\nSuccess rate:100%, DEX+1
            2022445, //  Scroll for Earring for DEX 60% - Improves DEX on earrings.\nSuccess rate:60%, DEX+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022446, //  Scroll for Earring for DEX 10% - Improves DEX on earrings.\nSuccess rate:10%, DEX+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022447, //  Scroll for Earring for LUK 100% - Improves LUK on earrings..\nSuccess rate:100%, LUK+1
            2022448, //  Scroll for Earring for LUK 70% - Improves LUK on earrings..\nSuccess rate:70%, LUK+2nIf failed, the item will be destroyed at a 50% rate.
            2022449, //  Scroll for Earring for LUK 60% - Improves LUK on earrings.\nSuccess rate:60%, LUK+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022450, //  Scroll for Earring for LUK 30% - Improves LUK on earrings..\nSuccess rate:30%, LUK+3nIf failed, the item will be destroyed at a 50% rate.
            2022451, //  Scroll for Earring for LUK 10% - Improves LUK on earrings.\nSuccess rate:10%, LUK+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022452, //  Scroll for Earring for HP 100% - Improves HP on earrings..\nSuccess rate:100%, MaxHP+5
            2022453, //  Scroll for Earring for HP 70% - Improves HP on earrings..\nSuccess rate:70%, MaxHP+15nIf failed, the item will be destroyed at a 50% rate.
            2022454, //  Scroll for Earring for HP 60% - Improves HP on earrings.\nSuccess rate:60%, MaxHP+15. The success rate of this scroll can be enhanced by Vega's Spell.
            2022455, //  Scroll for Earring for HP 30% - Improves HP on earrings..\nSuccess rate:30%, MaxHP+30nIf failed, the item will be destroyed at a 50% rate.
            2022456, //  Scroll for Earring for HP 10% - Improves HP on earrings.\nSuccess rate:10%, MaxHP+30. The success rate of this scroll can be enhanced by Vega's Spell.
            2022457, //  Scroll for Topwear for DEF - Improves weapon def. on topwear.\nSuccess rate:100%, weapon def.+1
            2022458, //  Scroll for Topwear for DEF - Improves weapon def. on topwear.\nSuccess rate:60%, weapon def.+2, magic def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022459, //  Scroll for Topwear for DEF - Improves weapon def. on topwear.\nSuccess rate:10%, weapon def. +5, magic def. +3, MaxHP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2022460, //  Scroll for Topwear for DEF - Improves weapon def. on topwear.\nSuccess rate:100%, weapon def. +5, magic def. +3, MaxHP+10
            2022461, //  Dark scroll for Topwear for DEF - Improves weapon def. on topwear.\nSuccess rate:70%, weapon def. +2, magic def. +1nIf failed, the item will be destroyed at a 50% rate.
            2022462, //  Dark scroll for Topwear for DEF - Improves weapon def. on topwear.\nSuccess rate:30%, weapon def. +5, magic def. +3, MaxHP+10nIf failed, the item will be destroyed at a 50% rate.
            2022463, //  Dark scroll for Topwear for STR - Improves STR on topwear.\nSuccess rate: 70%, STR + 2nIf failed, the item will be destroyed at a 50% rate.
            2022465, //  Dark scroll for Topwear for STR - Improves STR on topwear.\nSuccess rate: 30%, STR + 3nIf failed, the item will be destroyed at a 50% rate.
            2022466, //  Dark scroll for Topwear for HP - Improves HP on topwear.\nSuccess rate: 70%, MaxHP + 15nIf failed, the item will be destroyed at a 50% rate.
            2022467, //  Dark scroll for Topwear for HP - Improves HP on topwear.\nSuccess rate: 30%, MaxHP + 30nIf failed, the item will be destroyed at a 50% rate.
            2022468, //  Dark Scroll for Topwear for LUK - Improves LUK on the topwear.\nSuccess Rate 70%, LUK+2, avoidability+1nIf failed, the item will be destroyed at a 50% rate.
            2022476, //  Dark Scroll for Topwear for LUK - Improves LUK on the topwear.\nSuccess Rate 30%, LUK+3, avoidability+3nIf failed, the item will be destroyed at a 50% rate.
            2022477, //  Scroll for Topwear for LUK - Improves LUK on the topwear.\nSuccess Rate 10%, LUK+3, avoidability+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022478, //  Scroll for Topwear for LUK - Improves LUK on the topwear.\nSuccess Rate 60%, LUK+2, avoidability+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022479, //  Scroll for Topwear for LUK - Improves LUK on the topwear.\nSuccess Rate 100%, LUK+1
            2022480, //  Scroll for Topwear for DEF - Improves Weapon Def. on Topwear.\nSuccess rate: 65%, Weapon Def. +2, Magic Def. +1
            2022481, //  Scroll for Topwear for DEF - Improves Weapon Def. on Topwear.\nSuccess rate: 15%, Weapon Def. +5, Magic Def. +3, MaxHP +10
            2022482, //  Scroll for Topwear for STR 100% - Improves strength on topwear..Success rate 100%, STR+1
            2022496, //  Scroll for Topwear for STR 60% - Improves strength on topwear.\nSuccess rate 60%, STR+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022499, //  Scroll for Topwear for STR 10% - Improves strength on topwear.\nSuccess rate 10%, STR+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022500, //  Scroll for Topwear for HP 100% - Improves HP on topwear..Success rate 100%, MaxHP + 5
            2022501, //  Scroll for Topwear for HP 60% - Improves HP on topwear.\nSuccess rate 60%, MaxHP + 15. The success rate of this scroll can be enhanced by Vega's Spell.
            2022502, //  Scroll for Topwear for HP 10% - Improves HP on topwear.\nSuccess rate 10%, MaxHP + 30. The success rate of this scroll can be enhanced by Vega's Spell.
            2022503, //  Scroll for Topwear for LUK 100% - Improves luck on topwear..\nSuccess rate:100%, LUK+1
            2022504, //  Scroll for Topwear for LUK 70% - Improves luck on topwear..\nSuccess rate:70%, LUK+2nIf failed, the item will be destroyed at a 50% rate.
            2022505, //  Scroll for Topwear for LUK 60% - Improves luck on topwear.\nSuccess rate:60%, LUK+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022506, //  Scroll for Topwear for LUK 30% - Improves luck on topwear..\nSuccess rate:30%, LUK+3nIf failed, the item will be destroyed at a 50% rate.
            2022507, //  Scroll for Topwear for LUK 10% - Improves luck on topwear.\nSuccess rate:10%, LUK+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022508, //  Scroll for Overall Armor for DEX - Improves dexterity on the overall armor.\nSuccess rate:100%, DEX+1
            2022509, //  Scroll for Overall Armor for DEX - Improves dexterity on the overall armor.\nSuccess rate:60%, DEX+2, accuracy+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022510, //  Scroll for Overall Armor for DEX - Improves dexterity on the overall armor.\nSuccess rate:10%, DEX+5, accuracy+3, speed+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022511, //  Scroll for Overall Armor for DEF - Improves weapon def. on the overall armor.\nSuccess rate:100%, weapon def.+1
            2022512, //  Scroll for Overall Armor for DEF - Improves def. on the overall armor.\nSuccess rate:60%, weapon def.+2, magic def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022513, //  Scroll for Overall Armor for DEF - Improves def. on the overall armor.\nSuccess rate:10%, wepon def. +5, magic def. +3, MaxHP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2022514, //  Scroll for Overall Armor for DEX - Improves dexterity on the overall armor.\nSuccess rate:100%, DEX+5, accuracy+3, speed+1
            2022515, //  Scroll for Overall Armor for DEF - Improves weapon def. on the overall armor.\nSuccess rate:30%, weapon def.+5, magic def.+3, MaxHP+10
            2022516, //  Dark scroll for Overall Armor for DEX - Improves dexterity on the overall armor.\nSuccess rate:70%, DEX+2, accuracy+1nIf failed, the item will be destroyed at a 50% rate.
            2022517, //  Dark scroll for Overall Armor for DEX - Improves dexterity on the overall armor.\nSuccess rate:30%, DEX+4, accuracy+3, speed+1nIf failed, the item will be destroyed at a 50% rate.
            2022518, //  Dark scroll for Overall Armor for DEF - Improves weapon def. on the overall armor.\nSuccess rate:70%, weapon def.+2, magic def.+1nIf failed, the item will be destroyed at a 50% rate.
            2022519, //  Dark scroll for Overall Armor for DEF - Improves weapon def. on the overall armor.\nSuccess rate:30%, weapon def.+5, magic def.+3, MaxHP+10nIf failed, the item will be destroyed at a 50% rate.
            2022520, //  Scroll for Overall Armor for INT - Improves INT on the overall armor.\nSuccess rate: 100%, INT + 1
            2022521, //  Scroll for Overall Armor for INT - Improves INT on the overall armor.\nSuccess rate: 60%, INT + 2, magic def. +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022522, //  Scroll for Overall Armor for INT - Improves INT on the overall armor.\nSuccess rate: 10%, INT + 5, magic def. + 3, MaxMP + 10. The success rate of this scroll can be enhanced by Vega's Spell.
            2022523, //  Scroll for Overall Armor for LUK - Improves LUK on the overall armor.\nSuccess rate: 100%, LUK + 1
            2022524, //  Scroll for Overall Armor for LUK - Improves LUK on the overall armor.\nSuccess rate: 60%, LUK + 2, avoidability + 1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022526, //  Scroll for Overall Armor for LUK - Improves LUK on the overall armor.\nSuccess rate: 10%, LUK + 5, avoidability + 3, accuracy + 1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022527, //  Dark scroll for Overall Armor for INT - Improves INT on the overall armor.\nSuccess rate:70%, INT+2, magic defense+1nIf failed, the item will be destroyed at a 50% rate.
            2022528, //  Dark scroll for Overall Armor for INT - Improves INT on the overall armor.\nSuccess rate:30%, INT+5, magic defense+3, MaxMP+10nIf failed, the item will be destroyed at a 50% rate.
            2022529, //  Dark scroll for Overall Armor for LUK - Improves LUK on the overall armor.\nSuccess rate:70%, LUK+2, avoidability+1nIf failed, the item will be destroyed at a 50% rate.
            2022530, //  Dark scroll for Overall Armor for LUK - Improves LUK on the overall armor.\nSuccess rate:30%, LUK+5, avoidability+3, accuracy+1nIf failed, the item will be destroyed at a 50% rate.
            2022531, //  Scroll for Overall Armor for DEX - Improves DEX on Overall Armor.\nSuccess rate: 65%, DEX +2, Accuracy +1
            2022536, //  Scroll for Overall Armor for DEX - Improves DEX on Overall Armor.\nSuccess rate: 15%, DEX +5, Accuracy+3, Speed +1
            2022537, //  Overall Armor Scroll for DEF - Improves Weapon Def. on Overall Armor.\nSuccess rate: 65%, Weapon Def. +2, Magic Def. +1
            2022538, //  Overall Armor Scroll for DEF - Improves Weapon Def. on Overall Armor.\nSuccess rate: 15%, Weapon Def. +5, Magic Def. +3, MaxHP +10
            2022539, //  Scroll for Overall Armor for INT - Improves INT on the Overall Armor.\nSuccess rate: 65%, INT +2, Magic Def. +1
            2022540, //  Scroll for Overall Armor for INT - Improves INT on the overall armor.\nSuccess rate:15%, INT+5, magic def.+3, MaxMP+10
            2022541, //  Scroll for Overall Armor for LUK - Improves LUK on the overall armor.\nSuccess rate:65%, LUK+2, avoidability+1
            2022542, //  Scroll for Overall Armor for LUK - Improves LUK on the overall armor.\nSuccess rate:15%, LUK+5, avoidability+3, accuracy+1
            2022543, //  Scroll for Overall for STR 100% - Improves strength on overalls..\nSuccess rate:100%, STR+1
            2022544, //  Scroll for Overall for STR 70% - Improves strength on overalls..\nSuccess rate:70%, STR+2, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2022545, //  Scroll for Overall for STR 60% - Improves strength on overalls.\nSuccess rate:60%, STR+2, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022546, //  Scroll for Overall for STR 30% - Improves strength on overalls..\nSuccess rate:30%, STR+5, weapon def.+3, MaxHp+5nIf failed, the item will be destroyed at a 50% rate.
            2022547, //  Scroll for Overall for STR 10% - Improves strength on overalls.\nSuccess rate:10%, STR+5, weapon def.+3, MaxHP+5. The success rate of this scroll can be enhanced by Vega's Spell.
            2022548, //  Scroll for Bottomwear for DEF - Improves weapon def. on the bottomwear. nSuccess rate:100%, weapon def. +1
            2022549, //  Scroll for Bottomwear for DEF - Improves weapon def. on the bottomwear.\nSuccess rate:60%, weapon def. +2, magic def. +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022550, //  Scroll for Bottomwear for DEF - Improves weapon def. on the bottomwear.\nSuccess rate:10%, weapon def.+5, magic def.+3, MaxHP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2022552, //  Scroll for Bottomwear for DEF - Improves weapon def. on the bottomwear.\nSuccess rate:100%, weapon def.+5, magic def.+3, MaxHP+10
            2022553, //  Dark scroll for Bottomwear for DEF - Improves weapon def. on the bottomwear.\nSuccess rate:70%, weapon def.+2, magic def.+1nIf failed, the item will be destroyed at a 50% rate.
            2022554, //  Dark scroll for Bottomwear for DEF - Improves weapon def. on the bottomwear.\nSuccess rate: 30%, weapon def.+5, magic def. + 3, MaxHP + 10nIf failed, the item will be destroyed at a 50% rate.
            2022555, //  Dark scroll for Bottomwear for Jump - Improves jump on the bottomwear.\nSuccess rate: 70%, jump + 2, avoidability + 1nIf failed, the item will be destroyed at a 50% rate.
            2022556, //  Dark scroll for Bottomwear for Jump - Improves jump on the bottomwear.\nSuccess rate: 30%. jump + 4, avoidability + 2nIf failed, the item will be destroyed at a 50% rate.
            2022562, //  Dark scroll for Bottomwear for HP - Improves HP on the bottomwear.\nSuccess rate: 70%. MaxHP + 15nIf failed, the item will be destroyed at a 50% rate.
            2022563, //  Dark scroll for Bottomwear for HP - Improves HP on the bottomwear.\nSuccess rate: 30%. MaxHP + 30nIf failed, the item will be destroyed at a 50% rate.
            2022564, //  Dark Scroll for Bottomwear for DEX - Improves dexterity on the bottomwear.\nSuccess Rate 70%, DEX+2, speed+1nIf failed, the item will be destroyed at a 50% rate.
            2022570, //  Dark Scroll for Bottomwear for DEX - Improves dexterity on the bottomwear.\nSuccess Rate 30%, DEX+3, speed+3nIf failed, the item will be destroyed at a 50% rate.
            2022571, //  Scroll for Bottomwear for DEX - Improves dexterity on the bottomwear.\nSuccess Rate 10%, DEX+3, speed+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022572, //  Scroll for Bottomwear for DEX - Improves dexterity on the bottomwear.\nSuccess Rate 60%, DEX+2, speed+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022573, //  Scroll for Bottomwear for DEX - Improves dexterity on the bottomwear.\nSuccess Rate 100%, DEX+1
            2022574, //  Scroll for Bottomwear for DEF - Improves weapon def. on bottomwear.\nSuccess rate:65%, weapon def.+2, magic def.+1
            2022575, //  Scroll for Bottomwear for DEF - Improves weapon def. on bottomwear.\nSuccess rate:15%, weapon def.+5, magic def.+3, MaxHP+10
            2022576, //  Scroll for Bottomwear for Jump 100% - Improves jumping abilities on bottomwears..\nSuccess rate:100%, jump+1
            2022577, //  Scroll for Bottomwear for Jump 60% - Improves jumping abilities on bottomwears.\nSuccess rate:60%, jump+2, avoidability+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022578, //  Scroll for Bottomwear for Jump 10% - Improves jumping abilities on bottomwears..\nSuccess rate:10%, jump+4, avoidability+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2022579, //  Scroll for Bottomwear for HP 100% - Improves HP on bottomwears..\nSuccess rate:100%, MaxHP+5
            2022580, //  Scroll for Bottomwear for HP 60% - Improves HP on bottomwears.\nSuccess rate:60%, MaxHP+15. The success rate of this scroll can be enhanced by Vega's Spell.
            2022581, //  Scroll for Bottomwear for HP 10% - Improves HP on bottomwears.\nSuccess rate:10%, MaxHP+30. The success rate of this scroll can be enhanced by Vega's Spell.
            2022582, //  Scroll for Bottomwear for DEX 100% - Improves dexterity on bottomwears..\nSuccess rate:100%, DEX+1
            2022583, //  Scroll for Bottomwear for DEX 70% - Improves dexterity on bottomwears..\nSuccess rate:70%, DEX+2, accuracy+1nIf failed, the item will be destroyed at a 50% rate.
            2022584, //  Scroll for Bottomwear for DEX 60% - Improves dexterity on bottomwears.\nSuccess rate:60%, DEX+2, accuracy+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022585, //  Scroll for Bottomwear for DEX 30% - Improves dexterity on bottomwears..\nSuccess rate:30%, DEX+3, accuracy+2, speed+1nIf failed, the item will be destroyed at a 50% rate.
            2022586, //  Scroll for Bottomwear for DEX 10% - Improves dexterity on bottomwears.\nSuccess rate:10%, DEX+3, accuracy+2, speed+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022587, //  Scroll for Shoes for DEX - Improves dexterity on shoes.\nSuccess rate:100%, Avoidability+1
            2022588, //  Scroll for Shoes for DEX - Improves dexterity on shoes.\nSuccess rate:60%, Avoidability +2, Accuracy+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022613, //  Scroll for Shoes for DEX - Improves dexterity on shoes.\nSuccess rate:10%, Avoidability +5, accuracy +3, speed+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022614, //  Scroll for Shoes for Jump - Improves jump on shoes.\nSuccess rate:100%, jump +1
            2022615, //  Scroll for Shoes for Jump - Improves jump on shoes.\nSuccess rate: 60%, jump +2, DEX+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022616, //  Scroll for Shoes for Jump - Improves jump on shoes.\nSuccess rate:10%, jump+5, DEX+3, speed+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2022617, //  Scroll for Shoes for Speed - Improves speed on shoes.\nSuccess rate:100%, speed+1
            2022618, //  Scroll for Shoes for Speed - Improves speed on shoes.\nSuccess rate:60%, speed+2
            2022620, //  Scroll for Shoes for Speed - Improves speed on shoes.\nSuccess rate:10%, speed+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2022621, //  Scroll for Shoes for DEX - Improves DEX on shoes.\nSuccess rate:100%, avoidability+5, accuracy+3, speed+1
            2022622, //  Scroll for Shoes for Jump - Improves jump on shoes.\nSuccess rate:100%, jump+5, DEX+3, speed+1
            2022623, //  Scroll for Shoes for Speed - Improves speed on shoes.\nSuccess rate:100%, speed+3
            2022624, //  Dark scroll for Shoes for DEX - Improves DEX on shoes.\nSuccess rate:70%, avoidability+2, accuracy+1nIf failed, the item will be destroyed at a 50% rate.
            2022625, //  Dark scroll for Shoes for DEX - Improves DEX on shoes.\nSuccess rate:30%, avoidability+5, accuracy+3, speed+1nIf failed, the item will be destroyed at a 50% rate.
            2022626, //  Dark scroll for Shoes for Jump - Improves jump on shoes.\nSuccess rate:70%, jump+2, DEX+1nIf failed, the item will be destroyed at a 50% rate.
            2022627, //  Dark scroll for Shoes for Jump - Improves jump on shoes.\nSuccess rate:30%, jump+5, DEX+3, speed+1nIf failed, the item will be destroyed at a 50% rate.
            2022628, //  Dark scroll for Shoes for Speed - Improves speed on shoes.\nSuccess rate:70%, speed+2nIf failed, the item will be destroyed at a 50% rate.
            2022629, //  Dark scroll for Shoes for Speed - Improves speed on shoes.\nSuccess rate:30%, speed+3nIf failed, the item will be destroyed at a 50% rate.
            2022631, //  Scroll for Shoes for DEX - Improves dexterity on shoes.\nSuccess rate:65%, avoidability+2, accuracy+1
            2022632, //  Scroll for Shoes for DEX - Improves dexterity on shoes.\nSuccess rate:15%, avoidability+5, accuracy+3, speed+1
            2022633, //  Scroll for Jump for DEX - Improves jump on shoes.\nSuccess rate:65%, jump+2, DEX+1
            2022634, //  Scroll for Jump for DEX - Improves jump on shoes.\nSuccess rate:15%, jump+5, DEX+3, speed+1
            2030000, //  Scroll for Speed for DEX - Improves speed on shoes.\nSuccess rate:65%, speed+2
            2030001, //  Scroll for Speed for DEX - Improves speed on shoes.\nSuccess rate:15%, speed+3
            2030002, //  Scroll for Spikes on Shoes 10% - Adds traction to the shoes, which prevents the shoes from slipping on slippery surface.\nSuccess rate:10%, Does not affect the number of upgrades available. The success rate of this scroll can be enhanced by Vega's Spell.
            2030003, //  Scroll for Gloves for DEX - Improves dexterity on gloves.\nSuccess rate:100%, accurcacy +1
            2030004, //  Scroll for Gloves for DEX - Improves dexterity on gloves.\nSuccess rate: 60%, accuracy+2, DEX+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2030005, //  Scroll for Gloves for DEX - Improves dexterity on gloves.\nSuccess rate:10%, accuracy+5, DEX+3, avoidability+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2030006, //  Scroll for Gloves for ATT - Improves attack on gloves.\nSuccess rate:100%, weapon att. +1
            2030007, //  Scroll for Gloves for ATT - Improves attack on gloves.\nSuccess rate 60%, weapon att. +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2030008, //  Scroll for Gloves for ATT - Improves attack on gloves.\nSuccess rate:10%, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2030009, //  Scroll for Gloves for DEX - Improves DEX on the glove.\nSuccess rate:100%, accuracy+5, DEX+3, avoidability+1
            2030010, //  Scroll for Gloves for ATT - Improves weapon att. on the glove.\nSuccess rate:100%, weapon att.+3
            2030011, //  Dark scroll for Gloves for DEX - Improves DEX on the glove.\nSuccess rate:70%, accuracy+2, DEX+1nIf failed, the item will be destroyed at a 50% rate.
            2030012, //  Dark scroll for Gloves for DEX - Improves DEX on the glove.\nSuccess rate:30%, accuracy+5, DEX+3, avoidability+1nIf failed, the item will be destroyed at a 50% rate.
            2030016, //  Dark scroll for Gloves for ATT - Improves weapon att. on the glove.\nSuccess rate:70%, weapon att.+2nIf failed, the item will be destroyed at a 50% rate.
            2030019, //  Dark scroll for Gloves for ATT - Improves weapon att. on the glove.\nSuccess rate:30%, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2030020, //  Dark scroll for Gloves for HP - Improves HP on the glove.\nSuccess rate: 70%, MaxHP+15nIf failed, the item will be destroyed at a 50% rate.
            2031000, //  Dark scroll for Gloves for HP - Improves HP on the glove.\nSuccess rate: 30%, MaxHP + 30nIf failed, the item will be destroyed at a 50% rate.
            2031001, //  Dark Scroll for Gloves for Magic Att. - Improves magic attack on the glove.\nSuccess Rate 70%, magic attack+1, INT+1nIf failed, the item will be destroyed at a 50% rate.
            2031002, //  Dark Scroll for Gloves for Magic Att. - Improves magic attack on the glove.\nSuccess Rate 30%, magic attack+3, INT+3, magic defense+1nIf failed, the item will be destroyed at a 50% rate.
            2031003, //  Scroll for Gloves for Magic Att. - Improves magic attack on the glove.\nSuccess Rate 10%, magic defense+1, magic attack+3, INT+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2031004, //  Scroll for Gloves for Magic Att. - Improves magic attack on the glove.\nSuccess Rate 60%, magic attack+1, INT+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2031006, //  Scroll for Gloves for Magic Att. - Improves magic attack on the glove.\nSuccess Rate 100%, magic attack+1
            2031008, //  Scroll for Gloves for DEX - Improves dexterity on gloves.\nSuccess rate:65%, accuracy+2, DEX+1
            2032000, //  Scroll for Gloves for DEX - Improves dexterity on gloves.\nSuccess rate:15%, accuracy+5, DEX+3, avoidability+1
            2040000, //  Scroll for Gloves for ATT - Improves attack on gloves.\nSuccess rate:65%, weapon attack+2
            2040001, //  Scroll for Gloves for ATT - Improves attack on gloves.\nSuccess rate:15%, weapon attack+3
            2040002, //  Scroll for Gloves for HP 100% - Improves HP on gloves..\nSuccess rate:100%, MaxHP+5
            2040003, //  Scroll for Gloves for HP 60% - Improves HP on gloves.\nSuccess rate:60%, MaxHP+15. The success rate of this scroll can be enhanced by Vega's Spell.
            2040004, //  Scroll for Gloves for HP 10% - Improves HP on gloves.\nSuccess rate:10%, MaxHP+30. The success rate of this scroll can be enhanced by Vega's Spell.
            2040005, //  Scroll for Shield for DEF - Improves weapon def. on the shield.\nSuccess rate:100%, weapon def. +1
            2040006, //  Scroll for Shield for DEF - Improves weapon def. on the shield.\nSuccess rate:60%, weapon def.+2, magic def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040007, //  Scroll for Shield for DEF - Improves weapon def. on the shield.\nSuccess rate 10%, weapon def.+5, magic def.+3, MaxHP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2040008, //  Scroll for Shield for DEF - Improves weapon def. on the shield.\nSuccess rate 100%, weapon def.+5, magic def.+3, MaxHP+10
            2040009, //  Dark scroll for Shield for DEF - Improves weapon def. on the shield.\nSuccess rate 70%, weapon def.+2, magic def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040010, //  Dark scroll for Shield for DEF - Improves weapon def. on the shield.\nSuccess rate 30%, weapon def.+5, magic def.+3, MaxHP+10nIf failed, the item will be destroyed at a 50% rate.
            2040011, //  Dark scroll for Shield for LUK - Improves LUK on the shield.\nSuccess rate: 70%, LUK + 2nIf failed, the item will be destroyed at a 50% rate.
            2040012, //  Dark scroll for Shield for LUK - Improves LUK on the shield.\nSuccess rate: 30%, LUK + 3nIf failed, the item will be destroyed at a 50% rate.
            2040013, //  Dark scroll for Shield for HP - Improves HP on the shield.\nSuccess rate: 70%, MaxHP + 15nIf failed, the item will be destroyed at a 50% rate.
            2040014, //  Dark scroll for Shield for HP - Improves HP on the shield.\nSuccess rate: 30%, MaxHP + 30nIf failed, the item will be destroyed at a 50% rate.
            2040015, //  Scroll for Shield for DEF - Improves weapon defense on the shield.\nSuccess rate:65%, weapon def.+2, magic def.+1
            2040016, //  Scroll for Shield for DEF - Improves weapon defense on the shield.\nSuccess rate:15%, weapon def.+5, magic def.+3, MaxHP+10
            2040017, //  [4yrAnniv]Scroll for Shield for DEF - Improves weapon defense for Maple Magician shield, Maple warrior shield, and the Maple Shibus shield. nSuccess rate:40%, weapon def.+3, magic def.+2 nIf failed, the item will be destroyed at a 30% rate.
            2040018, //  Scroll for Shield for Weapon Att. - Improves weapon attack on the shield.\nSuccess Rate 60%, W. attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040019, //  Scroll for Shield for Weapon Att. - Improves weapon attack on the shield.\nSuccess Rate 10%, W. attack+3, STR+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040020, //  Dark Scroll for Shield for Weapon Att. - Improves weapon attack on the shield.\nSuccess Rate 70%, W. attack+2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040021, //  Dark Scroll for Shield for Weapon Att. - Improves weapon attack on the shield.\nSuccess Rate 30%, W. attack+3, STR+2nIf failed, the item will be destroyed at a 50% rate.
            2040022, //  Scroll for Shield for Magic Att. - Improves magic attack on the shield.\nSuccess Rate 100%, magic attack+1
            2040023, //  Scroll for Shield for Magic Att. - Improves magic attack on the shield.\nSuccess Rate 60%, magic attack+2, INT+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040024, //  Scroll for Shield for Magic Att. - Improves magic attack on the shield.\nSuccess Rate 10%, magic attack+3, INT+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040025, //  Dark Scroll for Shield for Magic Att. - Improves magic attack on the shield.\nSuccess Rate 70%, magic attack+2, INT+1nIf failed, the item will be destroyed at a 50% rate.
            2040026, //  Dark Scroll for Shield for Magic Att. - Improves magic attack on the shield.\nSuccess Rate 50%, magic attack+3, INT+2nIf failed, the item will be destroyed at a 50% rate.
            2040027, //  Scroll for Shield for LUK 100% - Improves LUK on shields..\nSuccess rate:100%, LUK+1
            2040028, //  Scroll for Shield for LUK 60% - Improves LUK on shields.\nSuccess rate:60%, LUK+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040029, //  Scroll for Shield for LUK 10% - Improves LUK on shields.\nSuccess rate:10%, LUK+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040030, //  Scroll for Shield for HP 100% - Improves HP on shields..\nSuccess rate:100%, MaxHP+5
            2040031, //  Scroll for Shield for HP 60% - Improves HP on shields.\nSuccess rate:60%, MaxHP+15. The success rate of this scroll can be enhanced by Vega's Spell.
            2040041, //  Scroll for Shield for HP 10% - Improves HP on shields.\nSuccess rate:10%, MaxHP+30. The success rate of this scroll can be enhanced by Vega's Spell.
            2040042, //  Scroll for Shield for STR 100% - Improves strength on shields..\nSuccess rate:100%, STR+1
            2040043, //  Scroll for Shield for STR 70% - Improves strength on shields..\nSuccess rate:70%, STR+2nIf failed, the item will be destroyed at a 50% rate.
            2040044, //  Scroll for Shield for STR 60% - Improves strength on shields.\nSuccess rate:60%, STR+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040045, //  Scroll for Shield for STR 30% - Improves strength on shields..\nSuccess rate:30%, STR+3nIf failed, the item will be destroyed at a 50% rate.
            2040046, //  Scroll for Shield for STR 10% - Improves strength on shields.\nSuccess rate:10%, STR+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040100, //  Scroll for Cape for Magic Def. - Improves magic def. on the cape.\nSuccess rate:100%, magic def. +1
            2040101, //  Scroll for Cape for Magic Def. - Improves magic def. on the cape.\nSuccess rate:60%, magic def.+3, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040102, //  Scroll for Cape for Magic Def. - Improves magic def. on the cape.\nSuccess rate:10%, magic def. +5, weapon def. +3, MaxMP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2040103, //  Scroll for Cape for Weapon Def. - Improves weapon def. on the cape.\nSuccess rate:100%, weapon def.+1
            2040104, //  Scroll for Cape for Weapon Def. - Improves weapon def. on the cape.\nSuccess rate:60%, weapon def.+3, magic def. +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040105, //  Scroll for Cape for Weapon Def. - Improves weapon def. on the cape.\nSuccess rate:10%, weapon def. +5, magic def.+3, MaxHP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2040106, //  Scroll for Cape for HP - Improves MaxHP on the cape.\nSuccess rate:100%, MaxHP+5
            2040107, //  Scroll for Cape for HP - Improves MaxHP on the cape.\nSuccess rate:60%, MaxHP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2040108, //  Scroll for Cape for HP - Improves MaxHP on the cape.\nSuccess rate:10%, MaxHP+20. The success rate of this scroll can be enhanced by Vega's Spell.
            2040109, //  Scroll for Cape for MP - Improves MaxMP on the cape.\nSuccess rate:100%, MaxMP+5
            2040110, //  Scroll for Cape for MP - Improves MaxMP on the cape.\nSuccess rate:60%, MaxMP+10. The success rate of this scroll can be enhanced by Vega's Spell.
            2040111, //  Scroll for Cape for MP - Improves MaxMP on the cape.\nSuccess rate:10%, MaxMP+20. The success rate of this scroll can be enhanced by Vega's Spell.
            2040112, //  Scroll for Cape for STR - Improves STR on the cape.\nSuccess rate:100%, STR+1
            2040113, //  Scroll for Cape for STR - Improves STR on the cape.\nSuccess rate:60%, STR+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040114, //  Scroll for Cape for STR - Improves STR on the cape.\nSuccess rate:10%, STR+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040115, //  Scroll for Cape for INT - Improves INT on the cape.\nSuccess rate:100%, INT+1
            2040116, //  Scroll for Cape for INT - Improves INT on the cape.\nSuccess rate:60%, INT+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040117, //  Scroll for Cape for INT - Improves INT on the cape.\nSuccess rate:10%, INT+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040118, //  Scroll for Cape for DEX - Improves DEX on the cape.\nSuccess rate:100%, DEX+1
            2040119, //  Scroll for Cape for DEX - Improves DEX on the cape.\nSuccess rate:60%, DEX+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040200, //  Scroll for Cape for DEX - Improves DEX on the cape.\nSuccess rate:10%, DEX+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040201, //  Scroll for Cape for LUK - Improves LUK on the cape.\nSuccess rate:100%, LUK+1
            2040202, //  Scroll for Cape for LUK - Improves LUK on the cape.\nSuccess rate:60%, LUK+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040203, //  Scroll for Cape for LUK - Improves LUK on the cape.\nSuccess rate:10%, LUK+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040204, //  Scroll for Cape for Magic Def. - Improves magic def. on the cape.\nSuccess rate:100%, magic def.+5, weapon def.+3, MaxMP+10
            2040205, //  Scroll for Cape for Weapon Def. - Improves weapon def. on the cape.\nSuccess rate:100%, weapon def.+5, magic def.+3, MaxHP+10
            2040206, //  Dark scroll for Cape for Magic Def. - Improves magic def. on the cape.\nSuccess rate:70%, magic def.+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040207, //  Dark scroll for Cape for Magic Def. - Improves magic def. on the cape.\nSuccess rate:30%, magic def.+5, weapon def.+3, MaxMP+10nIf failed, the item will be destroyed at a 50% rate.
            2040208, //  Dark scroll for Cape for Weapon Def. - Improves weapon def. on the cape.\nSuccess rate:70%, weapon def.+3, magic def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040209, //  Dark scroll for Cape for Weapon Def. - Improves weapon def. on the cape.\nSuccess rate:30%, weapon def.+5, magic def.+3, MaxHP+10nIf failed, the item will be destroyed at a 50% rate.
            2040300, //  Dark scroll for Cape for HP - Improves MaxHP on the cape.\nSuccess rate:70%, MaxHP+10nIf failed, the item will be destroyed at a 50% rate.
            2040301, //  Dark scroll for Cape for HP - Improves MaxHP on the cape.\nSuccess rate:30%, MaxHP+20nIf failed, the item will be destroyed at a 50% rate.
            2040302, //  Dark scroll for Cape for MP - Improves MaxMP on the cape.\nSuccess rate:70%, MaxMP+10nIf failed, the item will be destroyed at a 50% rate.
            2040303, //  Dark scroll for Cape for MP - Improves MaxMP on the cape.\nSuccess rate:30%, MaxMP+20nIf failed, the item will be destroyed at a 50% rate.
            2040304, //  Dark scroll for Cape for STR - Improves STR on the cape.\nSuccess rate:70%, STR+2nIf failed, the item will be destroyed at a 50% rate.
            2040305, //  Dark scroll for Cape for STR - Improves STR on the cape.\nSuccess rate:30%, STR+3nIf failed, the item will be destroyed at a 50% rate.
            2040306, //  Dark scroll for Cape for INT - Improves INT on the cape.\nSuccess rate:70%, INT+2nIf failed, the item will be destroyed at a 50% rate.
            2040307, //  Dark scroll for Cape for INT - Improves INT on the cape.\nSuccess rate:30%, INT+3nIf failed, the item will be destroyed at a 50% rate.
            2040308, //  Dark scroll for Cape for DEX - Improves DEX on the cape.\nSuccess rate:70%, DEX+2nIf failed, the item will be destroyed at a 50% rate.
            2040309, //  Dark scroll for Cape for DEX - Improves DEX on the cape.\nSuccess rate:30%, DEX+3nIf failed, the item will be destroyed at a 50% rate.
            2040310, //  Dark scroll for Cape for LUK - Improves LUK on the cape.\nSuccess rate:70%, LUK+2nIf failed, the item will be destroyed at a 50% rate.
            2040311, //  Dark scroll for Cape for LUK - Improves LUK on the cape.\nSuccess rate:30%, LUK+3nIf failed, the item will be destroyed at a 50% rate.
            2040312, //  Scroll for Cape for Magic DEF - Improves magic defense on the cape.\nSuccess rate:65%, magic def.+3, weapon def.+1
            2040313, //  Scroll for Cape for Magic DEF - Improves magic defense on the cape.\nSuccess rate:15%, magic def.+5, weapon def.+3, MaxMP+10
            2040314, //  Scroll for Cape for Weapon DEF - Improves weapon defense on the cape.\nSuccess rate:65%, weapon def.+3, magic def.+1
            2040315, //  Scroll for Cape for Weapon DEF - Improves weapon defense on the cape.\nSuccess rate:15%, weapon def.+5, magic def.+3, MaxHP+10
            2040316, //  Scroll for Cape for MaxHP - Improves MaxHP on the cape.\nSuccess rate:65%, MaxHP+10
            2040317, //  Scroll for Cape for MaxHP - Improves MaxHP on the cape.\nSuccess rate:15%, MaxHP+20
            2040318, //  Scroll for Cape for MP - Improves MaxMP on the cape.\nSuccess rate:65%, MaxMP+10
            2040319, //  Scroll for Cape for MP - Improves MaxMP on the cape.\nSuccess rate:15%, MaxMP+20
            2040320, //  Scroll for Cape for STR - Improves STR on the cape.\nSuccess rate:65%, STR+2
            2040321, //  Scroll for Cape for STR - Improves STR on the cape.\nSuccess rate:15%, STR+3
            2040322, //  Scroll for Cape for INT - Improves INT on the cape.\nSuccess rate:65%, INT+2
            2040323, //  Scroll for Cape for INT - Improves INT on the cape.\nSuccess rate:15%, INT+3
            2040324, //  Scroll for Cape for DEX - Improves dexterity on the cape.\nSuccess rate:65%, DEX+2
            2040325, //  Scroll for Cape for DEX - Improves dexterity on the cape.\nSuccess rate:15%, DEX+3
            2040326, //  Scroll for Cape for LUK - Improves LUK on the cape.\nSuccess rate:65%, LUK+2
            2040327, //  Scroll for Cape for LUK - Improves LUK on the cape.\nSuccess rate:15%, LUK+3
            2040328, //  Scroll for Cape for Cold Protection 10% - Includes the effect of protection from cold weather on the cape.\nSuccess rate: 10%. Does not affect the number of upgrades available. The success rate of this scroll can be enhanced by Vega's Spell.
            2040329, //  [4yrAnniv] Scroll for Cape for STR 20% - Improves strength on Maple Cape.\nSuccess rate:20%, STR+3 nIf failed, the item will be destroyed at a 30% rate.
            2040330, //  [4yrAnniv] Scroll for Cape for INT 20% - Improves INT on Maple Cape.\nSuccess rate:20%, INT+3 nIf failed, the item will be destroyed at a 30% rate.
            2040331, //  [4yrAnniv] Scroll for Cape for DEX 20% - Improves dexterity on Maple Cape.\nSuccess rate:20%, DEX+3 nIf failed, the item will be destroyed at a 30% rate.
            2040333, //  [4yrAnniv] Scroll for Cape for LUK 20% - Improves luck on Maple Cape.\nSuccess rate:20%, LUK+3 nIf failed, the item will be destroyed at a 30% rate.
            2040334, //  Dragon Stone - A powerful stone that contains the mysterious power of the dragon. Can only be used on Horntail Necklace.\nSuccess rate:100%, Weapon Defense +140, Magic Defense +140, Avoidability +15, All Stats +15
            2040335, //  Rock of Wisdom - Can only be used on Horus's Eye.\nSuccess rate:60%, HP +70, MP +70
            2040336, //  Scroll for One-Handed Sword for ATT - Improves attack on one-handed sword.\nSuccess rate:100%, weapon attack+1
            2040337, //  Scroll for One-Handed Sword for ATT - Improves attack on one-handed sword.\nSuccess rate:60%, weapon attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040338, //  Scroll for One-Handed Sword for ATT - Improves attack on one-handed sword.\nSuccess rate:10%, weapon attack+5, STR+3, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040339, //  Scroll for One-Handed Sword for ATT - Improves attack on one-handed sword.\nSuccess rate:100%, weapon attack+5, STR+3, weapon def.+1
            2040340, //  Dark scroll for One-Handed Sword for ATT - Improves attack on one-handed sword.\nSuccess rate:70%, weapon attack+2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040400, //  Dark scroll for One-Handed Sword for ATT - Improves attack on one-handed sword.\nSuccess rate:30%, weapon attack+5, STR+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040401, //  Dark Scroll for One-Handed Sword for Magic Att. - Improves magic attack on one-handed sword.\nSuccess Rate 70%, magic attack+1, INT+1nIf failed, the item will be destroyed at a 50% rate.
            2040402, //  Dark Scroll for One-Handed Sword for Magic Att. - Improves magic attack on one-handed sword.\nSuccess Rate 30%, magic attack+2, INT+2, magic defense+1nIf failed, the item will be destroyed at a 50% rate.
            2040403, //  Scroll for One-Handed Sword for Magic Att. - Improves magic attack on one-handed sword.\nSuccess Rate 10%, magic attack+2, magic defense+1, INT+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040404, //  Scroll for One-Handed Sword for Magic Att. - Improves magic attack on one-handed sword.\nSuccess Rate 60%, magic attack+1, INT+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040405, //  Scroll for One-Handed Sword for Magic Att. - Improves magic attack on one-handed sword.\nSuccess Rate 100%, magic attack+1
            2040406, //  Scroll for One-Handed Sword for ATT - Improves attack on the one-handed sword.\nSuccess rate:65%, weapon attack+2, STR+1
            2040407, //  Scroll for One-Handed Sword for ATT - Improves attack on the one-handed sword.\nSuccess rate:15%, weapon attack+5, STR+3, weapon def.+1
            2040408, //  [4yrAnniv]Scroll for One-Handed Sword for ATT - Improves attack for Maple Glory Sword. nSuccess rate:40%, weapon attack+3, STR+2 nIf failed, the item will be destroyed at a 30% rate.
            2040409, //  Scroll for One-Handed Sword for Accuracy 100% - Improves accuracy on one-handed swords.\nSuccess rate:100%, accuracy+1
            2040410, //  Scroll for One-Handed Sword for Accuracy 70% - Improves accuracy on one-handed swords.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate.
            2040411, //  Scroll for One-Handed Sword for Accuracy 60% - Improves accuracy on one-handed swords.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040412, //  Scroll for One-Handed Sword for Accuracy 30% - Improves accuracy on one-handed swords.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2040413, //  Scroll for One-Handed Sword for Accuracy 10% - Improves accuracy on one-handed swords.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040414, //  Scroll for One-Handed Axe for ATT - Improves attack on one-handed axe.\nSuccess rate:100%, weapon attack+1
            2040415, //  Scroll for One-Handed Axe for ATT - Improves attack on one-handed axe.\nSuccess rate:60%, weapon attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040416, //  Scroll for One-Handed Axe for ATT - Improves attack on one-handed axe.\nSuccess rate: 10%, weapon attack +5, STR+3, weapon def. +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040417, //  Scroll for One-Handed Axe for ATT - Improves attack on one-handed axe.\nSuccess rate:100%, weapon attack+5, STR+3, weapon def.+1
            2040418, //  Dark scroll for One-Handed Axe for ATT - Improves attack on one-handed axe.\nSuccess rate:70%, weapon attack+2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040419, //  Dark scroll for One-Handed Axe for ATT - Improves attack on one-handed axe.\nSuccess rate:30%, weapon attack+5, STR+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040420, //  Scroll for One-Handed Axe for ATT - Improves attack on the one-handed axe.\nSuccess rate:65%, weapon attack+2, STR+1
            2040421, //  Scroll for One-Handed Axe for ATT - Improves attack on the one-handed axe.\nSuccess rate:15%, weapon attack+5, STR+3, weapon def.+1
            2040422, //  [4yrAnniv]Scroll for One-Handed Axe for ATT - Improves attack on the Maple Steel Axe. nSuccess rate:40%, weapon attack+3, STR+2nIf failed, the item will be destroyed at a 30% rate.
            2040423, //  Scroll for One-Handed Axe for Accuracy 100% - Improves accuracy on one-handed axe.\nSuccess rate:100%, accuracy+1
            2040424, //  Scroll for One-Handed Axe for Accuracy 70% - Improves accuracy on one-handed axe.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate.
            2040425, //  Scroll for One-Handed Axe for Accuracy 60% - Improves accuracy on one-handed axe.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040426, //  Scroll for One-Handed Axe for Accuracy 30% - Improves accuracy on one-handed axe.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2040427, //  Scroll for One-Handed Axe for Accuracy 10% - Improves accuracy on one-handed axe.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040429, //  Scroll for One-Handed BW for ATT - Improves attack on one-handed blunt weapon.\nSuccess rate:100%, weapon attack+1
            2040430, //  Scroll for One-Handed BW for ATT - Improves attack on one-handed blunt weapon.\nSuccess rate:60%, weapon attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040431, //  Scroll for One-Handed BW for ATT - Improves attack on one-handed blunt weapon.\nSuccess rate: 10%, weapon attack +5, STR+3, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040432, //  Scroll for One-Handed BW for ATT - Improves attack on one-handed blunt weaponnSuccess rate:100%, weapon attack+5, STR+3, weapon def.+1
            2040433, //  Dark scroll for One-Handed BW for ATT - Improves attack on one-handed blunt weapon.\nSuccess rate:70%, weapon attack+2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040434, //  Dark scroll for One-Handed BW for ATT - Improves attack on one-handed blunt weapon.\nSuccess rate:30%, weapon attack+5, STR+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040435, //  Scroll for One-Handed BW for ATT - Improves attack on the one-handed blunt weapon.\nSuccess rate:65%, weapon attack+2, STR+1
            2040436, //  Scroll for One-Handed BW for ATT - Improves attack on the one-handed blunt weapon.\nSuccess rate:15%, weapon attack+5, STR+3, weapon def.+1
            2040500, //  [4yrAnniv]Scroll for One-Handed BW for ATT - Added attack upgrade option for the Maple Havoc Hammer. nSuccess rate:40%, weapon attack+3, STR+2nIf failed, the item will be destroyed at a 30% rate.
            2040501, //  Scroll for One-Handed BW for Accuracy 100% - Improves accuracy on one-handed blunt weapon.\nSuccess rate:100%, accuracy+1
            2040502, //  Scroll for One-Handed BW for Accuracy 70% - Improves accuracy on one-handed blunt weapon.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate.
            2040503, //  Scroll for One-Handed BW for Accuracy 60% - Improves accuracy on one-handed blunt weapon.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040504, //  Scroll for One-Handed BW for Accuracy 30% - Improves accuracy on one-handed blunt weapon.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2040505, //  Scroll for One-Handed BW for Accuracy 10% - Improves accuracy on one-handed blunt weapon.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040506, //  Scroll for Dagger for ATT - Improves attack on dagger.\nSuccess rate:100%, weapon attack+1
            2040507, //  Scroll for Dagger for ATT - Improves attack on dagger.\nSuccess rate:60%, weapon attack+2, LUK+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040508, //  Scroll for Dagger for ATT - Improves attack on dagger.\nSuccess rate: 10%, weapon attack +5, LUK+3, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040509, //  Scroll for Dagger for ATT - Improves attack on dagger.\nSuccess rate:100%, weapon attack+5, LUK+3, weapon def.+1
            2040510, //  Dark scroll for Dagger for ATT - Improves attack on dagger.\nSuccess rate:70%, weapon attack +2, LUK +1nIf failed, the item will be destroyed at a 50% rate.
            2040511, //  Dark scroll for Dagger for ATT - Improves attack on dagger.\nSuccess rate:30%, weapon attack +5, LUK +3, weapon def. +1nIf failed, the item will be destroyed at a 50% rate.
            2040512, //  Scroll for Dagger for ATT - Improves attack on the dagger.\nSuccess rate:65%, weapon attack+2, LUK+1
            2040513, //  Scroll for Dagger for ATT - Improves attack on the dagger.\nSuccess rate:15%, weapon attack+5, LUK+3, weapon def.+1
            2040514, //  [4yrAnniv]Scroll for Dagger for ATT - Improves attack on the Maple Dark Mate and Maple Asura DaggernSuccess rate:40%, weapon attack+3, LUK+2nIf failed, the item will be destroyed at a 30% rate.
            2040515, //  Scroll for Wand for Magic Att. - Improves magic on wand.\nSuccess rate:100%, magic attack+1
            2040516, //  Scroll for Wand for Magic Att. - Improves magic on wand.\nSuccess rate:60%, magic attack+2, INT+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040517, //  Scroll for Wand for Magic Att. - Improves magic on wand.\nSuccess rate:10%, magic attack+5, INT+3, magic def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040518, //  Scroll for Wand for Magic Att. - Improves magic on wand.\nSuccess rate:100%, magic attack+5, INT+3, magic def.+1
            2040519, //  Dark scroll for Wand for Magic Att. - Improves magic on wand.\nSuccess rate:70%, magic attack+2, INT+1nIf failed, the item will be destroyed at a 50% rate.
            2040520, //  Dark scroll for Wand for Magic Att. - Improves magic on wand.\nSuccess rate:30%, magic attack+5, INT+3, magic def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040521, //  Scroll for Wand for Magic Att. - Improves magic attack on the wand.\nSuccess rate:65%, magic attack+2, INT+1
            2040522, //  Scroll for Wand for Magic Att. - Improves magic attack on the wand.\nSuccess rate:15%, magic attack+5, INT+3, magic def.+1
            2040523, //  [4yrAnniv]Scroll for Wand for Magic Att. - Improves magic attack on Maple Shine Wand.\nSuccess rate:40%, magic attack+3, INT+2nIf failed, the item will be destroyed at a 30% rate.
            2040524, //  Scroll for Staff for Magic Att. - Improves magic on staff.\nSuccess rate:100%, magic attack+1
            2040525, //  Scroll for Staff for Magic Att. - Improves magic on staff.\nSuccess rate:60%, magic attack+2, INT+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040526, //  Scroll for Staff for Magic Att. - Improves magic on staff.\nSuccess rate:10%, magic attack+5, INT+3, magic def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040527, //  Scroll for Staff for Magic Att. - Improves magic on staff.\nSuccess rate:100%, magic attack+5, INT+3, magic def.+1
            2040528, //  Dark scroll for Staff for Magic Att. - Improves magic on staff.\nSuccess rate:70%, magic attack+2, INT+1nIf failed, the item will be destroyed at a 50% rate.
            2040529, //  Dark scroll for Staff for Magic Att. - Improves magic on staff.\nSuccess rate:30%, magic attack+5, INT+3, magic def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040530, //  Scroll for Staff for Magic Att. - Improves magic attack on the staff.\nSuccess rate:65%, magic attack+2, INT+1
            2040531, //  Scroll for Staff for Magic Att. - Improves magic attack on the staff.\nSuccess rate:15%, magic attack+5, INT+3, magic def.+1
            2040532, //  [4yrAnniv]Scroll for Staff for Magic Att. - Improves magic attack on Maple Wisdom Staff.\nSuccess rate:40%, magic attack+3, INT+2nIf failed, the item will be destroyed at a 30% rate.
            2040533, //  Scroll for Two-handed Sword for ATT - Improves attack on two-handed sword.\nSuccess rate:100%, weapon attack+1
            2040534, //  Scroll for Two-handed Sword for ATT - Improves attack on two-handed sword.\nSuccess rate:60%, weapon attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040538, //  Scroll for Two-handed Sword for ATT - Improves attack on two-handed sword.\nSuccess rate:10%, weapon attack+5, STR+3, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040539, //  Scroll for Two-handed Sword for ATT - Improves attack on two-handed sword weapon.\nSuccess rate:100%, weapon attack+5, STR+3, weapon def.+1
            2040540, //  Dark scroll for Two-handed Sword for ATT - Improves attack on two-handed sword.\nSuccess rate:70%, weapon attack+2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040541, //  Dark scroll for Two-handed Sword for ATT - Improves attack on two-handed sword.\nSuccess rate:30%, weapon attack+5, STR+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040542, //  Scroll for Two-Handed Sword for ATT - Improves attack on the two-handed sword.\nSuccess rate:65%, weapon attack+2, STR+1
            2040543, //  Scroll for Two-Handed Sword for ATT - Improves attack on the two-handed sword.\nSuccess rate:15%, weapon attack+5, STR+3, weapon def.+1
            2040600, //  [4yrAnniv]Scroll for Two-Handed Sword for ATT - Improves attack for Maple Soul Rohen. nSuccess rate:40%, weapon attack+3, STR+2nIf failed, the item will be destroyed at a 30% rate.
            2040601, //  Scroll for Two-Handed Sword for Accuracy 100% - Improves accuracy on two-handed swords.\nSuccess rate:100%, accuracy+1
            2040602, //  Scroll for Two-Handed Sword for Accuracy 70% - Improves accuracy on two-handed swords.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate.
            2040603, //  Scroll for Two-Handed Sword for Accuracy 60% - Improves accuracy on two-handed swords.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040604, //  Scroll for Two-Handed Sword for Accuracy 30% - Improves accuracy on two-handed swords.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2040605, //  Scroll for Two-Handed Sword for Accuracy 10% - Improves accuracy on two-handed swords.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040606, //  Scroll for Two-handed Axe for ATT - Improves attack on two-handed axe.\nSuccess rate:100%, weapon attack+1
            2040607, //  Scroll for Two-handed Axe for ATT - Improves attack on two-handed axe.\nSuccess rate:60%, weapon attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040608, //  Scroll for Two-handed Axe for ATT - Improves attack on two-handed axe.\nSuccess rate:10%, weapon attack+5, STR+3, weapon def. +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040609, //  Scroll for Two-handed Axe for ATT - Improves attack on two-handed axe.\nSuccess rate:100%, weapon attack+5, STR+3, weapon def.+1
            2040610, //  Dark scroll for Two-handed Axe for ATT - Improves attack on two-handed axe.\nSuccess rate:70%, weapon attack+2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040611, //  Dark scroll for Two-handed Axe for ATT - Improves attack on two-handed axe.\nSuccess rate:30%, weapon attack+5, STR+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040612, //  Scroll for Two-Handed Axe for ATT - Improves attack on the two-handed axe.\nSuccess rate:65%, weapon attack+2, STR+1
            2040613, //  Scroll for Two-Handed Axe for ATT - Improves attack on the two-handed axe.\nSuccess rate:15%, weapon attack+5, STR+3, weapon def.+1
            2040614, //  [4yrAnniv]Scroll for Two-Handed Axe for ATT - Improves attack for Maple Demon Axe. nSuccess rate:40%, weapon attack+3, STR+2nIf failed, the item will be destroyed at a 30% rate.
            2040615, //  Scroll for Two-Handed Axe for Accuracy 100% - Improves accuracy on two-handed axe.\nSuccess rate:100%, accuracy+1
            2040616, //  Scroll for Two-Handed Axe for Accuracy 70% - Improves accuracy on two-handed axe.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate.
            2040617, //  Scroll for Two-Handed Axe for Accuracy 60% - Improves accuracy on two-handed axe.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040618, //  Scroll for Two-Handed Axe for Accuracy 30% - Improves accuracy on two-handed axe.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2040619, //  Scroll for Two-Handed Axe for Accuracy 10% - Improves accuracy on two-handed axe.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040620, //  Scroll for Two-handed BW for ATT - Improves attack on two-handed blunt weapon.\nSuccess rate:100%, weapon attack+1
            2040621, //  Scroll for Two-handed BW for ATT - Improves attack on two-handed blunt weapon.\nSuccess rate:60%, weapon attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040622, //  Scroll for Two-handed BW for ATT - Improves attack on two-handed blunt weapon.\nSuccess rate:10%, weapon attack+5, STR+3, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040623, //  Scroll for Two-handed BW for ATT - Improves attack on two-handed blunt weapon.\nSuccess rate:100%, weapon attack+5, STR+3, weapon def.+1
            2040624, //  Dark scroll for Two-handed BW for ATT - Improves attack on two-handed blunt weapon.\nSuccess rate:70%, weapon attack+2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040625, //  Dark scroll for Two-handed BW for ATT - Improves attack on two-handed blunt weapon.\nSuccess rate:30%, weapon attack+5, STR+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040626, //  Scroll for Two-Handed BW for ATT - Improves attack on the two-handed blunt weapon.\nSuccess rate:65%, weapon attack+2, STR+1
            2040627, //  Scroll for Two-Handed BW for ATT - Improves attack on the two-handed blunt weapon.\nSuccess rate:15%, weapon attack+5, STR+3, weapon def.+1
            2040629, //  [4yrAnniv]Scroll for Two-Handed BW for ATT - Improves attack on Maple Belzet. nSuccess rate:40%, weapon attack+3, STR+2nIf failed, the item will be destroyed at a 30% rate.
            2040630, //  Scroll for Two-Handed BW for Accuracy 100% - Improves accuracy on two-handed blunt weapon.\nSuccess rate:100%, accuracy+1
            2040631, //  Scroll for Two-Handed BW for Accuracy 70% - Improves accuracy on two-handed blunt weapon.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate.
            2040632, //  Scroll for Two-Handed BW for Accuracy 60% - Improves accuracy on two-handed blunt weapon.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040633, //  Scroll for Two-Handed BW for Accuracy 30% - Improves accuracy on two-handed blunt weapon.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2040634, //  Scroll for Two-Handed BW for Accuracy 10% - Improves accuracy on two-handed blunt weapon.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040635, //  Scroll for Spear for ATT - Improves attack on spear.\nSuccess rate:100%, weapon attack+1
            2040636, //  Scroll for Spear for ATT - Improves attack on spear.\nSuccess rate:60%, weapon attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040700, //  Scroll for Spear for ATT - Improves attack on spear.\nSuccess rate:10%, weapon attack+5, STR+3, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040701, //  Scroll for Spear for ATT - Improves attack on spear.\nSuccess rate:100%, weapon attack +5, STR+3, weapon def.+1
            2040702, //  Dark scroll for Spear for ATT - Improves attack on spear.\nSuccess rate:70%, weapon attack +2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040703, //  Dark scroll for Spear for ATT - Improves attack on spear.\nSuccess rate:30%, weapon attack +5, STR+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040704, //  Scroll for Spear for ATT - Improves attack on the Spear. nSuccess rate:65%, weapon attack+2, STR+1
            2040705, //  Scroll for Spear for ATT - Improves attack on the Spear. nSuccess rate:15%, weapon attack+5, STR+3, weapon def.+1
            2040706, //  [4yrAnniv]Scroll for Spear for ATT - Improves attack on Maple Soul Spear. nSuccess rate:40%, weapon attack+3, STR+2nIf failed, the item will be destroyed at a 30% rate.
            2040707, //  Scroll for Spear for Accuracy 100% - Improves accuracy on spears.\nSuccess rate:100%, accuracy+1
            2040708, //  Scroll for Spear for Accuracy 70% - Improves accuracy on spears.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate.
            2040709, //  Scroll for Spear for Accuracy 60% - Improves accuracy on spears.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040710, //  Scroll for Spear for Accuracy 30% - Improves accuracy on spears.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2040711, //  Scroll for Spear for Accuracy 10% - Improves accuracy on spears.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040712, //  Scroll for Pole Arm for ATT - Improves attack on pole arm.\nSuccess rate:100%, weapon attack+1
            2040713, //  Scroll for Pole Arm for ATT - Improves attack on pole arm.\nSuccess rate:60%, weapon attack+2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040714, //  Scroll for Pole Arm for ATT - Improves attack on pole arm.\nSuccess rate:10%, weapon attack+5, STR+3, weapon def.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040715, //  Scroll for Pole Arm for ATT - Improves attack on pole arm.\nSuccess rate:100%, weapon attack +5, STR+3, weapon def.+1
            2040716, //  Dark scroll for Pole Arm for ATT - Improves attack on pole arm.\nSuccess rate:70%, weapon attack +2, STR+1nIf failed, the item will be destroyed at a 50% rate.
            2040717, //  Dark scroll for Pole Arm for ATT - Improves attack on pole arm.\nSuccess rate:30%, weapon attack +5, STR+3, weapon def.+1nIf failed, the item will be destroyed at a 50% rate.
            2040718, //  Scroll for Pole Arm for ATT - Improves attack on the Pole arm. nSuccess rate:65%, weapon attack+2, STR+1
            2040719, //  Scroll for Pole Arm for ATT - Improves attack on the Pole arm. nSuccess rate:15%, weapon attack+5, STR+3, weapon def.+1
            2040720, //  [4yrAnniv]Scroll for Pole Arm for ATT - Improves attack for Maple Karstan nSuccess rate:40%, weapon attack+3, STR+2nIf failed, the item will be destroyed at a 30% rate.
            2040721, //  Scroll for Pole-Arm for Accuracy 100% - Improves accuracy on pole-arms.\nSuccess rate:100%, accuracy+1
            2040722, //  Scroll for Pole-Arm for Accuracy 70% - Improves accuracy on pole-arms.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate.
            2040723, //  Scroll for Pole-Arm for Accuracy 60% - Improves accuracy on pole-arms.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040727, //  Scroll for Pole-Arm for Accuracy 30% - Improves accuracy on pole-arms.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate.
            2040728, //  Scroll for Pole-Arm for Accuracy 10% - Improves accuracy on pole-arms.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040729, //  Scroll for Bow for ATT - Improves attack on bow.\nSuccess rate:100%, weapon attack+1
            2040730, //  Scroll for Bow for ATT - Improves attack on bow.\nSuccess rate: 60%, weapon attack+2, accuracy +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040731, //  Scroll for Bow for ATT - Improves attack on bow.\nSuccess rate:10%, weapon attack+5, accuracy+3, DEX+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040732, //  Scroll for Bow for ATT - Improves attack on bow.\nSuccess rate:100%, weapon attack +5, accuracy +3, DEX+1
            2040733, //  Dark scroll for Bow for ATT - Improves attack on bow.\nSuccess rate:70%, weapon attack +2, accuracy +1nIf failed, the item will be destroyed at a 50% rate.
            2040734, //  Dark scroll for Bow for ATT - Improves attack on bow.\nSuccess rate:30%, weapon attack +5, accuracy +3, DEX+1nIf failed, the item will be destroyed at a 50% rate.
            2040735, //  Scroll for Bow for ATT - Improves attack on the Bow. nSuccess rate:65%, weapon attack+2, accuracy+1
            2040736, //  Scroll for Bow for ATT - Improves attack on the Bow. nSuccess rate:15%, weapon attack+5, accuracy+3, DEX+1
            2040737, //  [4yrAnniv]Scroll for Bow for ATT - Improves attack on Maple Kandiva Bow. nSuccess rate:40%, weapon attack+3, accuracy+1nIf failed, the item will be destroyed at a 30% rate.
            2040738, //  Scroll for Crossbow for ATT - Improves attack on crossbow.\nSuccess rate:100%, weapon attack+1
            2040739, //  Scroll for Crossbow for ATT - Improves attack on crossbow.\nSuccess rate:60%, weapon attack+2, accuracy+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040740, //  Scroll for Crossbow for ATT - Improves attack on crossbow.\nSuccess rate:10%, weapon attack+5, accuracy+3, DEX+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040741, //  Scroll for Crossbow for ATT - Improves attack on crossbow.\nSuccess rate:100%, weapon attack+5, accuracy+3, DEX+1
            2040742, //  Dark scroll for Crossbow for ATT - Improves attack on crossbow.\nSuccess rate:70%, weapon attack+2, accuracy+1nIf failed, the item will be destroyed at a 50% rate.
            2040755, //  Dark scroll for Crossbow for ATT - Improves attack on crossbow.\nSuccess rate:30%, weapon attack+5, accuracy+3, DEX+1nIf failed, the item will be destroyed at a 50% rate.
            2040756, //  Scroll for Crossbow for ATT - Improves attack on the Crossbow. nSuccess rate:65%, weapon attack+2, accuracy+1
            2040757, //  Scroll for Crossbow for ATT - Improves attack on the Crossbow. nSuccess rate:15%, weapon attack+5, accuracy+3, DEX+1
            2040758, //  [4yrAnniv]Scroll for Crossbow for ATT - Improves attack on Maple Nishada. nSuccess rate:40%, weapon attack+3, accuracy+2nIf failed, the item will be destroyed at a 30% rate.
            2040759, //  Scroll for Claw for ATT - Improves attack on claw.\nSuccess rate:100%, weapon attack+1
            2040760, //  Scroll for Claw for ATT - Improves attack on claw.\nSuccess rate:60%, weapon attack+2, accuracy+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040800, //  Scroll for Claw for ATT - Improves attack on claw.\nSuccess rate:10%, weapon attack+5, accuracy+3, LUK+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040801, //  Scroll for Claw for ATT - Improves attack on claw.\nSuccess rate:100%, weapon attack+5, accuracy+3, LUK+1
            2040802, //  Dark Scroll for Claw for ATT - Improves attack on claw.\nSuccess rate:70%, weapon attack +2, accuracy +1nIf failed, the item will be destroyed at the 50% rate.
            2040803, //  Dark scroll for Claw for ATT - Improves attack on claw.\nSuccess rate:30%, weapon attack +5, accuracy +3, LUK+1nIf failed, the item will be destroyed at a 50% rate.
            2040804, //  Scroll for Claw for ATT - Improves attack on the Claw. nSuccess rate:65%, weapon attack+2, accuracy+1
            2040805, //  Scroll for Claw for ATT - Improves attack on the Claw. nSuccess rate:15%, weapon attack+5, accuracy+3, LUK+1
            2040806, //  [4yrAnniv]Scroll for Claw for ATT - Improves attack on Maple Scandar. nSuccess rate:40%, weapon attack+3, accuracy+2nIf failed, the item will be destroyed at a 30% rate.
            2040807, //  Scroll for Knuckler for Attack 100% - Improves attack on Knucklers.\nSuccess rate:100%, weapon att. +1
            2040808, //  Scroll for Knuckler for Attack 60% - Improves attack on Knucklers.\nSuccess rate:60%, weapon att. +2, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040809, //  Scroll for Knuckler for ATT - Improves attack on Knucklers.\nSuccess rate:10%, weapon att. +5, STR+3, weapon def. +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040810, //  Scroll for Knuckler for Attack 70% - Improves attack on Knucklers.\nSuccess rate:70%, weapon att. +2, STR+1nIf failed, the item will be destroyed at a 50% rate
            2040811, //  Scroll for Knuckler for Attack 30% - Improves attack on Knucklers.\nSuccess rate:30%, weapon att. +5, STR+3, weapon def. +1nIf failed, the item will be destroyed at a 50% rate
            2040812, //  Scroll for Knuckle for Accuracy 100% - Improves accuracy on knuckles.\nSuccess rate:100%, accuracy+1
            2040813, //  Scroll for Knuckle for Accuracy 70% - Improves accuracy on knuckles.\nSuccess rate:70%, accuracy+3, DEX+2, weapon att.+1nIf failed, the item will be destroyed at a 50% rate..
            2040814, //  Scroll for Knuckle for Accuracy 60% - Improves accuracy on knuckles.\nSuccess rate:60%, accuracy+3, DEX+2, weapon att.+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040815, //  Scroll for Knuckle for Accuracy 30% - Improves accuracy on knuckles.\nSuccess rate:30%, accuracy+5, DEX+3, weapon att.+3nIf failed, the item will be destroyed at a 50% rate..
            2040816, //  Scroll for Knuckle for Accuracy 10% - Improves accuracy on knuckles.\nSuccess rate:10%, accuracy+5, DEX+3, weapon att.+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040817, //  [4yrAnniv] Scroll for Knuckle for Attack 40% - Improves the attack on Maple Golden Claw. nSuccess rate:40%,weapon att.+3,STR+2nIf failed, the item will be destroyed at a 30% rate.
            2040818, //  Scroll for Gun for Attack 100% - Improves attack on Guns.\nSuccess rate:100%, weapon att. +1
            2040819, //  Scroll for Gun for Attack 60% - Improves attack on Guns.\nSuccess rate:60%, weapon att. +2, accuracy+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040820, //  Scroll for Gun for ATT - Improves attack on Guns.\nSuccess rate:10%, weapon att. +5, accuracy+3, DEX+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040821, //  Scroll for Gun for Attack 70% - Improves attack on Guns.\nSuccess rate:70%, weapon att. +2, accuracy+1nIf failed, the item will be destroyed at a 50% rate
            2040822, //  Scroll for Gun for Attack 30% - Improves attack on Guns.\nSuccess rate:30%, weapon att. +5, accuracy+3, DEX+1nIf failed, the item will be destroyed at a 50% rate
            2040823, //  [4yrAnniv] Gun for Attack 40% - Improves the attact on Maple Canon Shooter. nSuccess rate:40%, weapon att.+3,naccuracy+2nIf failed, the item will be destroyed at a 30% rate.
            2040824, //  Scroll for Pet Equip. for Speed - Improves speed on pet equip.\nSuccess rate:100%, speed+1
            2040825, //  Scroll for Pet Equip. for Speed - Improves speed on pet equip.\nSuccess rate:60%, moving speed+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040826, //  Scroll for Pet Equip. for Speed - Improves speed on pet equip.\nSuccess rate:10%, moving speed+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040829, //  Scroll for Pet Equip. for Jump - Improves jump on pet equip.\nSuccess rate:100%, jump+1
            2040830, //  Scroll for Pet Equip. for Jump - Improves jump on pet equip.\nSuccess rate:60%, jump+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2040831, //  Scroll for Pet Equip. for Jump - Improves jump on pet equip.\nSuccess rate:10%, jump+3. The success rate of this scroll can be enhanced by Vega's Spell.
            2040832, //  Scroll for Speed for Pet Equip. - Improves speed on Pet Equip. nSuccess rate:65%, speed+2
            2040833, //  Scroll for Speed for Pet Equip. - Improves speed on Pet Equip. nSuccess rate:15%, speed+3
            2040834, //  Scroll for jump for Pet Equip. - Improves jump on Pet equip. nSuccess rate:65%, jump+2
            2040900, //  Scroll for jump for Pet Equip. - Improves jump on Pet equip. nSuccess rate:15%, jump+3
            2040901, //  Scroll for Pet Equip. for STR 60% - Improves strength on pet equipments.\nSuccess rate:60%, STR+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040902, //  Scroll for Pet Equip. for INT 60% - Improves intelligence on pet equipments.\nSuccess rate:60%, INT+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040903, //  Scroll for Pet Equip. for DEX 60% - Improves dexterity on pet equipments.\nSuccess rate:60%, DEX+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040904, //  Scroll for Pet Equip. for LUK 60% - Improves luck on pet equipments.\nSuccess rate:60%, LUK+1. The success rate of this scroll can be enhanced by Vega's Spell.
            2040905, //  Clean Slate Scroll 1% - Recovers the lost number of upgrades due to failed scroll by 1. Not available on Cash Items. Success rate:1%, If failed, the item will be destroyed at a 2% rate..
            2040906, //  Clean Slate Scroll 3% - Recovers the lost number of upgrades due to failed scroll by 1. Success rate:3%, If failed, the item will be destroyed at a 6% rate..
            2040907, //  Clean Slate Scroll 5% - Recovers the lost number of upgrades due to failed scroll by 1. Success rate:5%, If failed, the item will be destroyed at a 10% rate..
            2040908, //  Clean Slate Scroll 20% - Recovers the lost number of upgrades due to failed scroll by 1. Success rate:20%, If failed, the item will be destroyed at a 50% rate..
            2040909, //  Chaos Scroll 60% - Alters the equipment for better or worse. Not available on Cash Items.\nSuccess rate:60%
            2040910, //  Liar Tree Sap 100% - Use this on Pinocchio's nose to improve or downgrade the item options.\nSuccess rate:100%
            2040911, //  Maple Syrup 100% - Use it on Maple Leaf to improve or downgrade the item option. Success rate:100%
            2040912, //  Agent Equipment Scroll 100% - Can be used on an agent equipment to either enhance or worsen the function.\nSuccess rate:100%
            2040914, //  Antidote - Cures the state of being poisoned.
            2040915, //  Eyedrop - Cures the state of darkness
            2040916, //  Tonic - Cures the state of weakness.
            2040917, //  Holy Water - Allows you to recover from the state of curse or being sealed up.
            2040918, //  All Cure Potion - Allows you to recover from any abnormal state.
            2040919, //  One View - Use it on view-restricted map to increase the vision of your partymates and yourself for 1 minute.
            2040920, //  Owl Potion - Recover total vision in vision-restricted map for 30 seconds.
            2040921, //  The Lost eye - The Lost eye
            2040922, //  Flaming feather - Flaming feather
            2040923, //  Arrow for Bow - A barrel full of arrows. Only usable with bows.
            2040924, //  Bronze Arrow for Bow - A barrel full of bronze arrows. Only usable with bows.\nAttack + 1
            2040925, //  Steel Arrow for Bow - A barrel full of steel arrows. Only usable with bows.\nSTR +1, Attack +1
            2040926, //  Red Arrow for Bow - A case full of arrows. Can only be used with a bow. \nAttack + 4.
            2040927, //  Diamond Arrow for Bow - A case full of arrows. Can only be used with a bow.nAttack + 4.
            2040928, //  Snowball - A packed ball of snow. Can be thrown to inflict damage.
            2040929, //  Big Snowball - A bigger, more intimidating packed ball of snow. Can be thrown to inflict damage.
            2040930, //  Arrow for Crossbow - A barrel full of arrows. Only usable with crossbows.
            2040931, //  Bronze Arrow for Crossbow - A barrel full of bronze arrows. Only usable with crossbows.\nAttack + 1
            2040932, //  Steel Arrow for Crossbow - A barrel full of steel arrows. Only usable with crossbows.nAttack +2
            2040933, //  Blue Arrow for Crossbow - A case full of arrows. Can only be used with a crossbow.nAttack + 4.
            2040936, //  Diamond Arrow for Crossbow - A case full of arrows. Can only be used with a crossbow.nAttack + 4.
            2040937, //  Subi Throwing-Stars - A throwing-star made out of steel. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 15
            2040938, //  Wolbi Throwing-Stars - A throwing-star made out of steel. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 17
            2040939, //  Mokbi Throwing-Stars - A throwing-star made out of steel. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 19
            2040940, //  Kumbi Throwing-Stars - A throwing-star made out of steel. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 21
            2040941, //  Tobi Throwing-Stars - A throwing-star made out of steel. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 23
            2040942, //  Steely Throwing-Knives - A throwing-star made out of steel. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 25
            2040943, //  Ilbi Throwing-Stars - A throwing-star made out of steel. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 27
            2041000, //  Hwabi Throwing-Stars - A throwing-star made out of steel. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 27
            2041001, //  Snowball - A well-packed snowball. Once they run out, they need to be recharged.\nLevel Limit : 10, Attack + 17
            2041002, //  Wooden Top - When thrown, it spins fast and flies at great speed. Once they are all used up, they need to be recharged.\nLevel Limit : 10, Attack + 19
            2041003, //  Icicle - Sharp icicles. Once they run out, they need to be recharged.rnLevel Limit : 10, Attack + 21
            2041004, //  Maple Throwing-Stars - Maple-shaped steel throwing-stars. Once they run out, they need to be recharged.rnLevel Limit : 10, Attack + 21
            2041005, //  Paper Fighter Plane - A paper plane that can be thrown at things. Once they run out, they need to be recharged. Attack +20
            2041006, //  Orange - A tasty orange that can be thrown at things. Attack + 20
            2041007, //  Devil Rain Throwing Star - Throwing Star
            2041008, //  A Beginner Thief's Throwing Stars - These are steel throwing stars given by Dark Lord for Beginner Thieves. Unlike normal throwing stars, you can't recharge it. \nAttack + 15
            2041009, //  Crystal Ilbi Throwing-Stars - A throwing-star made of crystal. Once they run out, they need to be recharged. rnAttack + 29
            2041010, //  Balanced Fury - Ancient Shadowknight throwing stars made from black crystal.  These can be recharged when used up. rnAttack + 30
            2041011, //  Heart Megaphone - Shout to everyone in the world your character is on with this megaphone. (Heart accents)
            2041012, //  Skull Megaphone - Shout to everyone in the world your character is on with this megaphone. (Skull accents)
            2041013, //  Black Sack - If you think your level's too low, don't bother opening it.
            2041014, //  Monster Sack 1 - Summons weak monsters of level 10 and under
            2041015, //  Monster Sack 2 - Summons weak monsters between levels 10 and 20
            2041016, //  Monster Sack 3 - Summons mid-lower-leveled monsters between levels 20 and 30
            2041017, //  Monster Sack 4 - Summons mid-level monsters between levels 30 and 40
            2041018, //  Monster Sack 5 - Summons mid-leveled monsters between levels 40 and 50
            2041019, //  Monster Sack 6 - Summons high-leveled monsters between levels 50 and 60
            2041020, //  Monster Sack 7 - Summons high-leveled monsters between levels 60 and 70
            2041021, //  Summoning the Boss - To the old, the pregnant, and the low-leveled : don't even bother...
            2041022, //  Summoning New-Type Balrog - The moment you summon it...you're dead already
            2041023, //  Summoning "Dances with Balrog's Clone" - Summons Dances with Balrog's Clone
            2041024, //  Summoning Grendel the Really Old's Clone - Summons Grendel the Really Old's Clone
            2041025, //  Summoning Athena Pierce's Clone - Summons Athena Pierce's Clone
            2041026, //  Summoning Dark Lord's Clone - Summons Dark Lord's Clone
            2041027, //  Brand New Monster Galore - Bam!
            2041028, //  Summoning Bag of Birds - A bag, which summons blue and pink birds living in Eos Tower
            2041029, //  Different Sack - A sack that summons monsters.
            2041030, //  Alien Sack - A sack full of aliens
            2041031, //  Toy Robot Sack - A sack full of toy robots.
            2041032, //  Toy Trojan Sack - A sack full of toy trojans.
            2041033, //  Moon Sack - Make your wish in front of the full moon.
            2041034, //  Moon Sack - Make your wish in front of the full moon.
            2041035, //  Moon Sack - Make your wish in front of the full moon.
            2041036, //  Moon Sack - Make your wish in front of the full moon.
            2041037, //  Moon Sack - Make your wish in front of the full moon.
            2041038, //  Moon Sack - Make your wish in front of the full moon.
            2041039, //  Penalty Monster Sack 1 - Summons Black Knight.
            2041040, //  Penalty Monster Sack 2 - Summons Myst Knight.
            2041041, //  Summoning Three-Tail Fox - A peculiar summon sack that summons Three-Tail Fox
            2041042, //  Summoning Ghosts - A peculiar summon sack that summons ghosts. No way to tell which one, though...
            2041043, //  Summoning Goblins - A peculiar summon sack that summons goblins. No way to tell which one, though...
            2041044, //  Summoning Horntail A - Summons Head A of Horntail.
            2041045, //  Summoning Horntail C - Summons Head C of Horntail.
            2041046, //  Monster Sack 8 - Summons high-leveled monsters between levels 70 and 80
            2041047, //  Monster Sack 9 - Summons high-leveled monsters between levels 80 and 90
            2041048, //  Monster Sack 10 - Summons high-leveled monsters between levels 90 and 100
            2041049, //  Monster Sack 11 - Summons high-leveled monsters between levels 100 and 110
            2041050, //  Summon Master Monsters 1 - Summon the Event-only Mano & Stumpy.
            2041051, //  Summon Master Monsters 2 - Summon the Event-only Faust, King Clang, Timer, and Dyle.
            2041052, //  Summon Master Monsters 3 - Summon the Event-only Nine-Tailed Fox, Tae Roon, and King Sage Cat.
            2041053, //  Summon Master Monsters 4 - Summon the Event-only Elliza, and Snowman.
            2041054, //  Summoning Lord Pirate - Summons Lord Pirate.
            2041055, //  Summoning Peeking Lord Pirate - Summons Peeking Lord Pirate.
            2041056, //  Summoning Angry Lord Pirate - Summons Angry Lord Pirate.
            2041057, //  Summoning Enraged Lord Pirate - Summons Enraged Lord Pirate.
            2041058, //  Summoning Lord Pirate's Jar - Summoning Lord Pirate's Jar.
            2041059, //  Summoning Lord Pirate's Ginseng Jar - Summons Lord Pirate's Ginseng Jar.
            2041060, //  Summoning Lord Pirate's Bellflower - Summons Lord Pirate's Bellflower.
            2041061, //  Summoning Lord Pirate's Old Bellflower - Summoning Lord Pirate's Old Bellflower.
            2041062, //  Summoning Lord Pirate's Mr. Alli - Summons Lord Pirate's Mr. Alli
            2041066, //  Summoning Lord Pirate's Kru - Summons Lord Pirate's Kru.
            2041067, //  Independence Day Firecracker 1 - This firecracker has been specially made to celebrate July 4th, 1776, our Independence Day. For 10 minutes, Speed and Jump increase by 5.
            2041068, //  Independence Day Firecracker 2 - This firecracker has been specially made to celebrate July 4th, 1776, our Independence Day. For 10 minutes, Attack and Magic increase by 5.
            2041069, //  Independence Day Firecracker 3 - This firecracker has been specially made to celebrate July 4th, 1776, our Independence Day. For 10 minutes, Def. and Magic Def. increase by 10.
            2041100, //  Red Potion for Noblesse - A special potion made out of herbs that exclusively grow in Ereve. Made specifically for Noblesss. nHP +50.
            2041101, //  Blue Potion for Noblesse - A special potion made out of herbs that exclusively grow in Ereve. Made specifically for Noblesss. nMP +100.
            2041102, //  Fish Net with a Catch - The fish net seems to have caught something. Let's double-click it to check its content.
            2041103, //  Big Belly Fish - A fish that apparently swallowed something. Let's double-click on the fish to check its content.
            2041104, //  Blessing of the Forest - With the purified forest raining gold rain, all affected will receive a temporary boost on speed and jump.
            2041105, //  Fire Grill Skewer - A delicious holiday food made out of beef, mushroom, and bellflower on a skewer.
            2041106, //  Sweet Rice Cake - A sweet, tasty rice cake. Recovers 1,500 HP and MP.
            2041107, //  Sweet Rice Cake - A sweet, tasty rice cake. ATT +8, MP +8 for 15 minutes.
            2041108, //  Sweet Rice Cake - A sweet, tasty rice cake. For 15 min., Speed +5, Jump +3.
            2041109, //  Increases Physical Attack Rat. - An item that only works inside Mu Lung Dojo.
            2041110, //  Increases Magic Attack Rate. - An item that only works inside Mu Lung Dojo.
            2041111, //  Increases Physical Defense Rate. - An item that only works inside Mu Lung Dojo.
            2041112, //  Increases Magic Defense Rate. - An item that only works inside Mu Lung Dojo.
            2041113, //  Increases Accuracy - An item that only works inside Mu Lung Dojo.
            2041114, //  Increases Avoidability - An item that only works inside Mu Lung Dojo.
            2041115, //  Increases Speed - An item that only works inside Mu Lung Dojo.
            2041116, //  Increases Max HP - An item that only works inside Mu Lung Dojo.
            2041117, //  Increases Max MP - An item that only works inside Mu Lung Dojo.
            2041118, //  Increases Physical Attack Rate - An item that only works inside Mu Lung Dojo.
            2041119, //  Increases Magic Attack Rate - An item that only works inside Mu Lung Dojo.
            2041200, //  Increases Physical Defense Rate - An item that only works inside Mu Lung Dojo.
            2041211, //  Increases Magic Defense Rate - An item that only works inside Mu Lung Dojo.
            2041212, //  Increases Accuracy - An item that only works inside Mu Lung Dojo.
            2041300, //  Increases Avoidability - An item that only works inside Mu Lung Dojo.
            2041301, //  Increases Speed - An item that only works inside Mu Lung Dojo.
            2041302, //  Increases MaxHP - An item that only works inside Mu Lung Dojo.
            2041303, //  Increases Max MP - An item that only works inside Mu Lung Dojo.
            2041304, //  Increases Physical Attack Rate - An item that only works inside Mu Lung Dojo.
            2041305, //  Increases Magic Attack Rate - An item that only works inside Mu Lung Dojo.
            2041306, //  Increases Physical Defense Rate - An item that only works inside Mu Lung Dojo.
            2041307, //  Increases Magic Defense Rate - An item that only works inside Mu Lung Dojo.
            2041308, //  Increases Accuracy - An item that only works inside Mu Lung Dojo.
            2041309, //  Increases Avoidability - An item that only works inside Mu Lung Dojo.
            2041310, //  Increases Speed - An item that only works inside Mu Lung Dojo.
            2041311, //  Increases Max HP - An item that only works inside Mu Lung Dojo.
            2041312, //  Increases Max MP - An item that only works inside Mu Lung Dojo.
            2041313, //  Increases Physical Attack Rate - An item that only works inside Mu Lung Dojo.
            2041314, //  Increases Magic Attack Rate - An item that only works inside Mu Lung Dojo.
            2041315, //  Increases Physical Defense Rate - An item that only works inside Mu Lung Dojo.
            2041316, //  Increases Magic Defense Rate - An item that only works inside Mu Lung Dojo.
            2041317, //  Increases Accuracy - An item that only works inside Mu Lung Dojo.
            2041318, //  Increases Avoidability - An item that only works inside Mu Lung Dojo.
            2041319, //  Increases Speed - An item that only works inside Mu Lung Dojo.
            2043000, //  Increases Max HP - An item that only works inside Mu Lung Dojo.
            2043001, //  Increases Max MP - An item that only works inside Mu Lung Dojo.
            2043002, //  Increases Physical Attack Rate - An item that only works inside Mu Lung Dojo.
            2043003, //  Increases Magic Attack Rate - An item that only works inside Mu Lung Dojo.
            2043004, //  Increases Physical Defense Rate - An item that only works inside Mu Lung Dojo.
            2043005, //  Increases Magic Defense Rate - An item that only works inside Mu Lung Dojo.
            2043006, //  Increases Accuracy - An item that only works inside Mu Lung Dojo.
            2043007, //  Increases Avoidability - An item that only works inside Mu Lung Dojo.
            2043008, //  Increases Speed - An item that only works inside Mu Lung Dojo.
            2043009, //  Increases Max HP - An item that only works inside Mu Lung Dojo.
            2043010, //  Increases Max MP - An item that only works inside Mu Lung Dojo.
            2043011, //  Increases Physical Attack Rate - An item that only works inside Mu Lung Dojo.
            2043012, //  Increases Magic Attack Rate - An item that only works inside Mu Lung Dojo.
            2043013, //  Increases Physical Defense Rate - An item that only works inside Mu Lung Dojo.
            2043015, //  Increases Magic Defense Rate - An item that only works inside Mu Lung Dojo.
            2043016, //  Increases Accuracy - An item that only works inside Mu Lung Dojo.
            2043017, //  Increases Avoidability - An item that only works inside Mu Lung Dojo.
            2043018, //  Increases Speed - An item that only works inside Mu Lung Dojo.
            2043019, //  Increases Max HP - An item that only works inside Mu Lung Dojo.
            2043021, //  Increases Max MP - An item that only works inside Mu Lung Dojo.
            2043022, //  Increases Physical Attack Rate - An item that only works inside Mu Lung Dojo.
            2043023, //  Increases Magic Attack Rate - An item that only works inside Mu Lung Dojo.
            2043024, //  Increases Physical Defense Rate - An item that only works inside Mu Lung Dojo.
            2043025, //  Increases Magic Defense Rate - An item that only works inside Mu Lung Dojo.
            2043100, //  Increases Accuracy - An item that only works inside Mu Lung Dojo.
            2043101, //  Increases Avoidability - An item that only works inside Mu Lung Dojo.
            2043102, //  Increases Speed - An item that only works inside Mu Lung Dojo.
            2043103, //  Increases Max HP - An item that only works inside Mu Lung Dojo.
            2043104, //  Increases Max MP - An item that only works inside Mu Lung Dojo.
            2043105, //  Small Stories - A tape that contains various small stories from daily life. You can fine view its content by double-clicking on it.
            2043106, //  Gaga's Appreciation - Gaga's appreciation. For an hour, your attack and magic rate will go up 20, defense rate 100, accuracy and avoidability 50, and speed and jump ability will go up 10.
            2043107, //  Gaga's Appreciation - Gaga's appreciation. For 20 minutes, your attack rate and magic will go up 10, defense rate 30, accuracy and avoidability 20, and speed and jump ability will go up 3.
            2043108, //  Mysterious Box - A box with something mysterious inside. I should open it to see what it could be. If it's my lucky day, I might find an awesome gift inside.n#cYou can open it by double-clicking on it.#
            2043110, //  Protective Shield - It can only be used in Mu Lung Dojo. It blocks an attack up to 3 times.
            2043111, //  Mu Lung Dojo Mana Elixir - It recovers your MP.
            2043112, //  Mu Lung Dojo Elixir - It recovers 50% of your HP and MP.
            2043113, //  Mu Lung Dojo Power Elixir - It recovers both HP and MP.
            2043114, //  Mu Lung Dojo Cure-All Medicine - It recovers any status error.
            2043116, //  Warm and Fuzzy Winter - Weapon Att +20, Magic Att +30 for 15 minutes.
            2043117, //  Maze Reward - The EXP awarded by completing the maze created by Richie Gold.
            2043118, //  EXP Increase(S) - Provides 50 Bonus EXP.
            2043119, //  EXP Increase(M) - Provides 200 Bonus EXP.
            2043120, //  EXP Increase(L) - Provides 500 Bonus EXP.
            2043200, //  Happy New Year! - Weapon Att +20, Magic Att +30 for 15 min.
            2043201, //  Elixir - A legendary potion.\nRecovers 50% of HP and MP.
            2043202, //  Power Elixir - A legendary potion.nFully recovers HP and MP.
            2043203, //  Shinsoo's Blessing - For 1 hour, Weapon Att +5, MP +10, Weapon DEF +20, Magic DEF +20, Speed +10.
            2043204, //  Cassandra's Reward 1 - For 1 hour, Meso Drop Rate +30%.
            2043205, //  Cassandra's Reward 2 - For 40 minutes, Meso Drop Rate +50%.
            2043206, //  Cassandra's Reward 3 - For 30 min., Meso Drop Rate 2x.
            2043207, //  Cassandra's Reward 4 - For 1 hour, Item Drop Rate +50%.
            2043208, //  Cassandra's Reward 5 - For 30 min., Item Drop Rate 2x.
            2043210, //  Heartpounding Box - A box in which no one has a clue what's in it. If the luck in is your side, then a beautiful present might be in store.n#cDouble-click to open.#
            2043211, //  Heartpounding Box - A box in which no one has a clue what's in it. If the luck in is your side, then a beautiful present might be in store.n#cDouble-click to open.#
            2043212, //  Heartpounding Box - A box in which no one has a clue what's in it. If the luck in is your side, then a beautiful present might be in store.n#cDouble-click to open.#
            2043213, //  Heartpounding Box - A box in which no one has a clue what's in it. If the luck in is your side, then a beautiful present might be in store.n#cDouble-click to open.#
            2043214, //  Invitation to the Moon - An invitation to the moon, sent by the Moon Bunnies. Using this will send you directly to Moon Bunny's ????.
            2043216, //  Invitation to the Nest - An invitation to the baby bird's nest sent by Gaga. Use it to be sent directly to the nest.
            2043217, //  Invitation to Ereve - An invitation to Ereve from Neinheart. This invitation will allow you to instantly move to Ereve.
            2043218, //  Richie Gold's Strange Lamp - An unbelievable lamp made by Richie Gold. Use this, and you'll be led somewhere in in the maze. No one knows exactly where you'll be sent, though.
            2043219, //  Red-Nose STR Bandage 60% - Improves STR on Rudolf's Red Nose.\nSuccess Rate:60%, STR +1
            2043220, //  Red-Nose DEX Bandage 60% - Improves DEX on Rudolf's Red Nose.\nSuccess Rate:60%, DEX +1
            2043300, //  Red-Nose INT Bandage 60% - Improves INT on Rudolf's Red Nose.\nSuccess Rate:60%, INT +1
            2043301, //  Red-Nose LUK Bandage 60% - Improves LUK on Rudolf's Red Nose.\nSuccess Rate:60%, LUK +1
            2043302, //  Red-Nose ATT Bandage 60% - Improves ATT on Rudolf's Red Nose.\nSuccess Rate:60%, ATT +1
            2043303, //  Red-Nose Weapon DEF Bandage 60% - Improves Weapon DEF on Rudolf's Red Nose.\nSuccess Rate:60%, Weapon DEF+1
            2043304, //  Red-Nose MP Bandage 60% - Improves MP on Rudolf's Red Nose.\nSuccess Rate:60%, MP+1
            2043305, //  Red-Nose Magic DEF Bandage 60% - Improves Magic DEF on Rudolf's Red Nose.\nSuccess Rate:60%, Magic DEF+1
            2043306, //  Red-Nose Avoidability Bandage 60% - Improves Avoidability on Rudolf's Red Nose.\nSuccess Rate:60%, Avoidability+1
            2043307, //  Red-Nose Accuracy Bandage 60% - Improves Accuracy on Rudolf's Red Nose.\nSuccess Rate:60%, Accuracy+1
            2043308, //  Scroll for Knuckles for ATT 65% - Improves ATT on Knuckles.\nSuccess Rate:65%, Weapon Att+2, STR+1
            2043311, //  Scroll for Knuckles for ATT 15% - Improves ATT on Knuckles.\nSuccess Rate:15%, Weapon Att+5, STR+3, Weapon DEF+1
            2043312, //  Scroll for Knuckles for Accuracy 65% - Improves Accuracy on Knuckles.\nSuccess Rate:65%, Accuracy+3, DEX+2, Weapon Att+1
            2043313, //  Scroll for Knuckles for Accuracy 15% - Improves Accuracy on Knuckles.\nSuccess Rate:15%, Accuracy+5, DEX+3, Weapon Att+3
            2043700, //  Gun ATT Scroll 65% - Improves ATT on guns.\nSuccess Rate:65%, Weapon Att.+2, Accuracy+1
            2043701, //  Gun ATT Scroll 15% - Improves ATT on guns.\nSuccess Rate:15%, Weapon Att.+5, Accuracy+3, DEX+1
            2043702, //  Beach Sandals Scroll 100% - Used on limited-edition beach sandals, with the options of improving/decreasing the stats.\nSuccess rate:100%
            2043703, //  Carnival Point 1 - Enhances CP 3.
            2043704, //  Carnival Point 2 - Enhances CP 3.
            2043705, //  Carnival Point 3 - Enhances CP 3.
            2043706, //  Elixir - A legendary potion.\nRecovers about 50% of HP and MP. (Exclusively for Monster Carnival)
            2043707, //  Power Elixir - A legendary Potion.\nRecovers all HP and MP. (Exclusively for Monster Carnival)
            2043708, //  Mana Elixir - A legendary potion.\nRecovers about 300 MP. (Exclusively for Monster Carnival)
            2043711, //  All-Cure Potion - Recovers the character from any abnormal state. (Exclusively for Monster Carnival)
            2043712, //  Spiegelmann's Marble - Can only be used on Spiegelmann's Necklace.\nSuccess Rate: 60%, HP +30, MP +30
            2043713, //  Scroll for Claw for ATT 100% - Improves attack on claw.\nSuccess rate:100%, weapon attack+2, accuracy+3
            2043800, //  Scroll for Crossbow for ATT 100% - Improves attack on crossbow.\nSuccess rate:100%, weapon attack+2, accuracy+3
            2043801, //  Scroll for Bow for ATT 100% - Improves attack on bow.\nSuccess rate: 100%, weapon attack+2, accuracy +3
            2043802, //  Scroll for Pole Arm for ATT 100% - Improves attack on pole arm.\nSuccess rate:100%, weapon attack+2, STR+2
            2043803, //  Scroll for Spear for ATT 100% - Improves attack on spear.\nSuccess rate:100%, weapon attack+2, STR+2
            2043804, //  Scroll for Two-handed BW for ATT 100% - Improves attack on two-handed blunt weapon.\nSuccess rate:100%, weapon attack+2, STR+2
            2043805, //  Scroll for Two-handed Axe for ATT 100% - Improves attack on two-handed axe.\nSuccess rate:100%, weapon attack+2, STR+2
            2043806, //  Scroll for Two-handed Sword for ATT 100% - Improves attack on two-handed sword.\nSuccess rate:100%, weapon attack+2, STR+2
            2043807, //  Scroll for Staff for Magic ATT 100% - Improves magic on staff.\nSuccess rate:100%, magic attack+2, INT+2
            2043808, //  Scroll for Wand for Magic ATT 100% - Improves magic on wand.\nSuccess rate:100%, magic attack+2, INT+2
            2043811, //  Scroll for Dagger for ATT 100% - Improves attack on dagger.\nSuccess rate:100%, weapon attack+2, LUK+2
            2043812, //  Scroll for One-Handed BW for ATT 100% - Improves attack on one-handed blunt weapon.\nSuccess rate:100%, weapon attack+2, STR+2
            2043813, //  Scroll for One-Handed Axe for ATT 100% - Improves attack on one-handed axe.\nSuccess rate:100%, weapon attack+2, STR+2
            2044000, //  Scroll for One-Handed Sword for ATT 100% - Improves attack on one-handed sword.\nSuccess rate:100%, weapon attack+2, STR+2
            2044001, //  Scroll for Cape for Magic DEF 100% - Improves magic def. on the cape.\nSuccess rate:100%, magic def.+3, weapon def.+2
            2044002, //  Scroll for Cape for Weapon DEF 100% - Improves weapon def. on the cape.\nSuccess rate:100%, weapon def.+3, magic def. +2
            2044003, //  Scroll for Shield for DEF 100% - Improves weapon def. on the shield.\nSuccess rate:100%, weapon def.+2, magic def.+3
            2044004, //  Scroll for Gloves for DEX 100% - Improves dexterity on gloves.\nSuccess rate: 100%, accuracy+2, DEX+2
            2044005, //  Scroll for Gloves for ATT 100% - Improves attack on gloves.\nSuccess rate 100%, weapon att. +2
            2044006, //  Scroll for Shoes for DEX 100% - Improves dexterity on shoes.\nSuccess rate:100%, Avoidability +2, Accuracy+3
            2044007, //  Scroll for Shoes for Jump 100% - Improves jump on shoes.\nSuccess rate: 100%, jump +2, DEX+2
            2044008, //  Scroll for Shoes for Speed 100% - Improves speed on shoes.\nSuccess rate:100%, speed+2
            2044010, //  Scroll for Bottomwear for DEF 100% - Improves weapon def. on the bottomwear.\nSuccess rate:100%, weapon def. +2, magic def. +3
            2044011, //  Scroll for Overall Armor for DEX 100% - Improves dexterity on the overall armor.\nSuccess rate:100%, DEX+2, accuracy+3
            2044012, //  Scroll for Overall Armor for DEF 100% - Improves def. on the overall armor.\nSuccess rate:100%, weapon def.+2, magic def.+3
            2044013, //  Scroll for Topwear for DEF 100% - Improves weapon def. on topwear.\nSuccess rate:100%, weapon def.+2, magic def.+3
            2044014, //  Scroll for Earring for INT 100% - Improves INT on ear accessory.\nSuccess rate:100%, magic attack +2, INT+2
            2044015, //  Scroll for Helmet for DEF 100% - Improves helmet def.\nSuccess rate:100%, weapon def.+2, magic def., +3
            2044024, //  Scroll for Helmet for HP 100% - Improves MaxHP on hats.\nSuccess rate:100%, MaxHP+15
            2044025, //  Mysterious Maple - The excitement of waiting for an adventure makes you feel good. Weapon and Magic Defense +10.
            2044026, //  Pigmy's Wings - Pigmy's Wings can be used to grant Speed +8.
            2044027, //  Cassandra's Star - Blessings from the star given to explorers who participated in the Starlight Festival. Weapon and Magic Defense +5 for 15 minutes.
            2044028, //  Point Improvement Treasure Chest - Improves point acquisition in the Biscuit Map.
            2044100, //  Point Increase Treasure Chest - Improves point acquisition in the Biscuit Map for 1 minute.
            2044101, //  Geppetto's Writing Analysis - Your Speed and Jump skills have increased because Geppetto has deciphered the letters.
            2044102, //  Hero's Gladius - Tristan's Powerful Strength
            2044103, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044104, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044105, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044106, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044107, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044108, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044110, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044111, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044112, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044113, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044114, //  Golden Pig's Shiny Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044116, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044117, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044118, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044119, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044120, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044200, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044201, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044202, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044203, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044204, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044205, //  Golden Pig's Dazzling Egg - An egg from a Golden Pig. You cannot tell what's inside. Double-click to crack it open.
            2044206, //  Azalea - A spring flower that especially enjoys sunlight. Restores approximately 2000 HP.
            2044207, //  Forsythia - The first flower to bloom in the spring. Restores approximately 2000 HP.
            2044208, //  Clover - A spring flower that symbolizes good luck. Restores approximately 2000 HP.
            2044210, //  Meaning of Azaleas - The meaning of Azaleas is love. 2x Meso drops for 30 minutes.
            2044211, //  Meaning of Forsythias - The meaning of Forsythias is hope. 2X item drops for 30 minutes.
            2044212, //  Meaning of Clovers - The meaning of Clovers in luck.  2X item drops for 1 hour.
            2044213, //  Underground Temple's Seal - The Seal's energy has been placed on the Underground Temple in order to trap the Balrog inside. It restricts the abilites of all living things.
            2044214, //  Gladius' Strength - The abilites of the person wieldng the Hero's Gladius is amplified under the protection of Tristan. Weapon ATT +30 and Magic ATT +30.
            2044216, //  Cry of a Little Lamb - Wolves slow down when they hear a lamb cry.
            2044217, //  Danger Escape - Movement speed increased for 3 seconds.
            2044218, //  Self Protection - Protects from a Wolf's attack 1 time.
            2044219, //  Little Lamb's Surprise Attack - Attacks a Wolf's back, temporarily immobolizing it.
            2044220, //  Great Confusion - Causes the sheep to become confused and lose direction.
            2044300, //  Sound of the Sheep's Bells - Slows wolves' movement speed.
            2044301, //  Sound of the Wolf's Bells - Sheep are temporarily unable to move.
            2044302, //  Wolf's Threat - Intimidates the sheep, making them weaker.
            2044303, //  Sheep Ranch Golden ? Egg - A golden egg that can only be obtained at the Sheep Ranch. It has a question mark engraved on it. Double-click to find out what's inside.
            2044304, //  6th Anniversary Gift Box - A gift box celebrating Maple Story's 6th Anniversary. What could be inside? rn#cCan be opened by double-clicking.#
            2044305, //  Crackers's Buff - A buff that Crackers has placed on the Witch's Treasure. Weapon and Magic Attack +10, Defense +20, Accuracy +20, and Avoidability +20 for 40 minutes.
            2044306, //  Artifact Hunt Encouragement Buff - A buff to encourage you upon accumulating 2,500 points in the Artifact Hunt. Attack +3, Magic Attack +6, Speed +6
            2044307, //  Artifact Hunt Encouragement Buff - A buff to encourage you upon accumulating 4,000 points in the Artifact Hunt. Attack +5, Magic Attack +10, Speed +15
            2044308, //  Family Studio Photo Coupon - A coupon that allows you to attend the 6th Anniversary Family Photo Shoot Event.
            2044310, //  Scroll for Earring for DEX 10% - Improves DEX on earrings. nSuccess rate: 10%, Dex +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2044311, //  Scroll for Earring for INT 10% - Improves INT on earrings. nSuccess rate: 10%, Magic ATT +5, INT +3, Magic Defense +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2044312, //  Scroll for Earring for LUK 10% - Improves LUK on earrings. nSuccess rate: 10%, LUK +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2044313, //  Balrog's STR Scroll 30% - Improves STR on Balrog Leather Shoes. nSuccess rate: 30%, STR +2
            2044314, //  Balrog's INT Scroll 30% - Improves INT on Balrog Leather Shoes. nSuccess rate: 30%, INT +2
            2044316, //  Balrog's LUK Scroll 30% - Improves LUK on Balrog Leather Shoes. nSuccess rate: 30%, LUK +2
            2044317, //  Balrog's DEX Scroll 30% - Improves DEX on Balrog Leather Shoes. nSucess rate: 30%, DEX +2
            2044318, //  Balrog's HP Scroll 30% - Improves HP on Balrog Leather Shoes. nSuccess rate: 30%, MaxHP +30
            2044319, //  Balrog's MP Scroll 30% - Improves MP on Balrog Leather Shoess. nSuccess rate: 30%. MaxMP +30
            2044320, //  Balrog's Speed Scroll 30% - Improves Speed on Balrog Leather Shoes. nSuccess rate: 30%, Speed +3
            2044400, //  Balrog's Jump Scroll 30% - Improves Jump on Balrog Leather Shoes. nSuccess rate: 30%, Jump +3
            2044401, //  Balrog's Accuracy Scroll 30% - Improves Accuracty on Balrog Leather Shoes. nSuccess rate: 30%, Accuracy +5
            2044402, //  Balrog's Avoidability Scroll 30% - Improves Avoidability on Balrog Leather Shoes. nSuccess rate: 30%, Avoidability +5
            2044403, //  Balrog's Defense Scroll 30% - Improves Defense on Balrog Leather Shoes. nSuccess rate: 30%, Weapons Defense +10, Magic Defense +10
            2044404, //  Balrog's Twilight Scroll 5% - Improves the function of Balrog Leather Shoes.\nSuccess rate: 5%, STR +4, INT +4, DEX +4, LUK +4, Speed +4, Jump +4, Avoidability +4, Accuracy +4, Weapons Defense +14, Magic Defense +14, MaxHP +40, MaxMP +40
            2044405, //  Scroll for Gloves for ATT 60% - Improves ATT on Gloves.\nSuccess rate: 60%, Weapons ATT +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044406, //  Scroll for Ring for STR 100% - Improves STR on Rings. nSuccess rate: 100%, STR +1
            2044407, //  Scroll for Rings for STR 60% - Improves STR on Rings.\nSuccess rate: 60%, STR +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044408, //  Scroll for Rings for STR 10% - Improves STR on Rings.\nSuccess rate: 10%, STR +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2044410, //  Scroll for Rings for INT 100% - Improves INT on Rings. nSuccess rate: 100%, INT +1
            2044411, //  Scroll for Rings for INT 60% - Improves INT on Rings.\nSuccess rate: 60%, INT +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044412, //  Scroll for Rings for INT 10% - Improves INT on Rings.\nSuccess rate: 10%, INT +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044413, //  Scroll for Rings for DEX 100% - Improves DEX on Rings. nSuccess rate: 100%, DEX +1
            2044414, //  Scroll for Rings for DEX 60% - Improves DEX on Rings.\nSuccess rate: 100%, DEX +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044416, //  Scroll for Rings for DEX 10% - Improves DEX on Rings.\nSuccess rate: 10%, DEX +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2044417, //  Scroll for Rings for LUK 100% - Improves LUK on Rings. nSuccess rate: 100%, LUK +1
            2044418, //  Scroll for Rings for LUK 60% - Improves LUK on Rings.\nSuccess rate: 60%, LUK+2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044419, //  Scroll for Rings for LUK 10% - Improves LUK on Rings.\nSuccess rate: 10%, LUK +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2044420, //  Dark Scroll for Rings for STR 70% - Improves STR on Rings. nSuccess rate: 70%, STR +2nIf unsuccessful, item has a 50% chance of being destroyed.
            2044500, //  Dark Scroll for Rings for STR 30% - Improves STR on Rings. nSuccess rate: 30%, STR +3nIf unsuccessful, item has a 50% chance of being destroyed.
            2044501, //  Dark Scroll for Rings for INT 70% - Improves INT on Rings.\nSuccess rate: 70%, INT +2nIf unsuccessful, item has a 50% chance of being destroyed.
            2044502, //  Dark Scroll for Rings for INT 30% - Improves INT on Rings.\nSuccess rate: 30%, INT +3nIf unsuccessful, item has a 50% chance of being destroyed.
            2044503, //  Dark Scroll for Rings for DEX 70% - Improves DEX on Rings.\nSuccess rate: 70%, DEX +2nIf unsuccessful, item has a 50% chance of being destroyed.
            2044504, //  Dark Scroll for Rings for DEX 30% - Improves DEX on Rings. nSuccess rate: 30%, DEX +3nIf unsuccessful, item has a 50% chance of being destroyed.
            2044505, //  Dark Scroll for Rings for LUK 70% - Improves LUK on Rings. nSuccess rate: 70%, LUK +2nIf unsuccessful, item has a 50% chance of being destroyed.
            2044506, //  Dark Scroll for Rings for LUK 30% - Improves LUK on Rings. nSuccess rate: 30%, LUK +3nIf unsuccessful, item has a 50% chance of being destroyed.
            2044507, //  Scroll for Belts for STR 100% - Improves STR on Belts. nSuccess rate: 100%, STR +1
            2044508, //  Scroll for Belts for STR 60% - Improves STR on Belts.\nSuccess rate: 60%, STR +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044511, //  Scroll for Belts for STR 10% - Improves STR on Belts.\nSuccess rate: 10%, STR +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2044512, //  Scroll for Belts for INT 100% - Improves INT on Belts. nSuccess rate: 100%, INT +1
            2044513, //  Scroll for Belts for INT 60% - Improves INT on Belts.\nSuccess rate: 60%, INT +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044600, //  Scroll for Belts for INT 10% - Improves INT on Belts.\nSuccess rate: 10%, INT +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2044601, //  Scroll for Belts for DEX 100% - Improves DEX on Belts. nSuccess rate: 100%, DEX +1
            2044602, //  Scroll for Belts for DEX 60% - Improves DEX on Belts.\nSuccess rate: 60%, DEX +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044603, //  Scroll for Belts for DEX 10% - Improves DEX on Belts.\nSuccess rate: 10%, DEX +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044604, //  Scroll for Belts for LUK 100% - Improves LUK on Belts. nSuccess rate: 100%, LUK +1
            2044605, //  Scroll for Belts for LUK 60% - Improves LUK on Belts.\nSuccess rate: 60%, LUK +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2044606, //  Scroll for Belts for LUK 10% - Improves LUK on Belts.\nSuccess rate: 10%, LUK +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2044607, //  Dark Scroll for Belts for STR 70% - Improves STR on Belts. nSuccess rate: 70%, STR +2nIf unsuccessful, item has a 50% chance of being destroyed.
            2044608, //  Dark Scroll for Belts for STR 30% - Improves STR on Belts. nSuccess rate: 30%, STR +3nIf unsuccessful, item has a 50% chance of being destroyed.
            2044611, //  Dark Scroll for Belts for INT 70% - Improves INT on Belts. nSuccess rate: 70%, INT +2nIf unsuccessful, item has a 50% chance of being destroyed.
            2044612, //  Dark Scroll for Belts for INT 30% - Improves INT on Belts. nSuccess rate: 30%, INT +3nIf unsuccessful, item has a 50% chance of being destroyed.
            2044613, //  Dark Scroll for Belts for DEX 70% - Improves DEX on Belts. nSuccess rate: 70%, DEX +2nIf unsuccessful, item has a 50% chance of being destroyed.
            2044700, //  Dark Scroll for Belts for DEX 30% - Improves DEX on Belts. nSuccess rate: 30%, DEX +3nIf unsuccessful, item has a 50% chance of being destroyed.
            2044701, //  Dark Scroll for Belts for LUK 70% - Improves LUK on Belts. nSuccess rate: 70%, LUK +2nIf unsuccessful, item has a 50% chance of being destroyed.
            2044702, //  Dark Scroll for Belts for LUK 30% - Improves LUK on Belts. nSuccess rate: 30%, LUK +3nIf unsuccessful, item has a 50% chance of being destroyed.
            2044703, //  Scroll for Two-Handed Swords for ATT 10% - Improves ATT on Two-Handed Swords. nSuccess rate: 10%, Weapons ATT +5, STR +3, Weapons Defense +1
            2044704, //  [6th Anniversary] Dark Scroll for Gloves for ATT 70% - Improves ATT on Gloves. nSuccess rate: 70%, Weapons ATT +1nIf unsuccessful, item has a 50% chance of being destroyed.
            2044705, //  [6th Anniversary] Dark Scroll for Gloves for ATT 30% - Improves ATT on Gloves. nSucces rate: 30%, Weapons ATT +2nIf unsuccessful, item has an 80% chance of being destroyed.
            2044706, //  [6th Anniversary] Dark Scroll for Gloves for STR 70%   [ - Improves STR on Gloves. nSuccess rate: 70%, Accuracy +2, STR +1nIf unsuccessful, item has a 50% chance of being destroyed.
            2044707, //  [6th Anniversary] Dark Scroll for Gloves for LUK 70% - Improves LUK on Gloves. nSuccess rate: 70%, Accuracy +2, LUK +1nIf unsuccessful, item has a 50% chance of being destroyed.
            2044708, //  [6th Anniversary] Dark Scroll for Gloves for INT 70% - Improves INT on Gloves. nSuccess rate: 70%, Accuracy +2, INT +1nIf unsuccessful, item has a 50% chance of being destroyed.
            2044711, //  [6th Anniversary] Dark Scroll for Gloves for DEX 70% - Improves DEX on Gloves. nSuccess rate: 70%, Accuracy +2, DEX +1nIf unsuccessful, item has a 50% chance of being destroyed.
            2044712, //  Chicken Kapitan - A popular Malaysian dish. This mild curry contains chicken pieces with rich coconut milk, onions and spices. \nRecovers 4000 HP.
            2044713, //  Mee Siam - A popular Malaysian dish made up of thin rice noodles. Served with salted soy beans, dried bean curd, boiled egg and tamarind. \nRecovers 4000 MP.
            2044800, //  Rojak - This famous dish is served with generous amounts of sweet thick, spicy peanut sauce with bean curds, prawn fritters, hard-boiled eggs and bean sprouts.\nRecovers 1000 HP and 1000 MP.
            2044801, //  Kangkung belacan - This spicy vegetable dish is served with KanKung (water spinach) and spicy sambal.nImproves Weapon Attack and Magic Attack +8 for 10 minutes.
            2044802, //  Kuih - These sweet, bite-sized delights are a favorite pastry dish served during parties and just for tea.nImproves Speed +10 for 30 minutes.
            2044803, //  Scroll for Claw for ATT 50% - Improves attack on claw.\nSuccess rate:50%, weapon attack+5, LUK+1, DEX+1
            2044804, //  Scroll for Crossbow for ATT 50% - Improves attack on crossbow.\nSuccess rate:50%, weapon attack+5, DEX+1, STR+1
            2044805, //  Scroll for Bow for ATT 50% - Improves attack on bow.\nSuccess rate:50%, weapon attack +5, DEX+1, STR+1
            2044806, //  Scroll for Pole Arm for ATT 50% - Improves attack on pole arm.\nSuccess rate:50%, weapon attack+5, STR+3, DEX+1
            2044807, //  Scroll for Spear for ATT 50% - Improves attack on spear.\nSuccess rate:50%, weapon attack+5, STR+3, DEX+1
            2044808, //  Scroll for Two-handed BW for ATT 50% - Improves attack on two-handed blunt weapon.\nSuccess rate:50%, weapon attack+5, STR+3, DEX+1
            2044809, //  Scroll for Two-handed Axe for ATT 50% - Improves attack on two-handed axe.\nSuccess rate:50%, weapon attack+5, STR+3, DEX+1
            2044810, //  Scroll for Two-handed Sword for ATT 50% - Improves attack on two-handed sword.\nSuccess rate:50%, weapon attack+5, STR+3, DEX+1
            2044811, //  Scroll for Staff for Magic Att. 50% - Improves magic on staff.\nSuccess rate:50%, magic attack+5, INT+3, LUK+1
            2044812, //  Scroll for Wand for Magic Att. 50% - Improves magic on wand.\nSuccess rate:50%, magic attack+5, INT+3, LUK+1
            2044813, //  Scroll for Dagger for ATT 50% - Improves attack on dagger.\nSuccess rate: 50%, weapon attack +5, LUK+3, DEX+1
            2044814, //  Scroll for One-Handed BW for ATT 50% - Improves attack on one-handed blunt weapon.\nSuccess rate: 50%, weapon attack +5, STR+3, DEX+1
            2044815, //  Scroll for One-Handed Axe for ATT 50% - Improves attack on one-handed axe.\nSuccess rate: 50%, weapon attack +5, STR+3, DEX+1
            2044816, //  Scroll for One-Handed Sword for ATT 50% - Improves attack on one-handed sword.\nSuccess rate:50%, weapon attack+5, STR+3, DEX+1
            2044817, //  Scroll for Cape for Magic Def. 50% - Improves magic def. on the cape.\nSuccess rate:50%, magic def. +5, weapon def. +4
            2044900, //  Scroll for Cape for Weapon Def. 50% - Improves weapon def. on the cape.\nSuccess rate:50%, weapon def. +5, magic def.+4
            2044901, //  Scroll for Shield for DEF 50% - Improves weapon def. on the shield.\nSuccess rate 50%, weapon def.+5, magic def.+4
            2044902, //  Scroll for Gloves for DEX 50% - Improves dexterity on gloves.\nSuccess rate:50%, accuracy+3, DEX+3, avoidability+2
            2044903, //  Scroll for Gloves for ATT 50% - Improves attack on gloves.\nSuccess rate:50%, weapon att.+3
            2044904, //  Scroll for Shoes for DEX 50% - Improves dexterity on shoes.\nSuccess rate:50%, Avoidability +3, accuracy +3, speed+2
            2044905, //  Scroll for Shoes for Jump 50% - Improves jump on shoes.\nSuccess rate:50%, jump+6, speed+1
            2044906, //  Scroll for Shoes for Speed 50% - Improves speed on shoes.\nSuccess rate:50%, speed+3, jump+1
            2044907, //  Scroll for Bottomwear for DEF 50% - Improves weapon def. on the bottomwear.\nSuccess rate:50%, weapon def.+5, magic def.+4
            2044908, //  Scroll for Overall Armor for DEX 50% - Improves dexterity on the overall armor.\nSuccess rate:50%, DEX+5, avoidability+1, speed+1
            2044909, //  Scroll for Overall Armor for DEF 50% - Improves def. on the overall armor.\nSuccess rate:50%, wepon def. +5, magic def. +4
            2044910, //  Scroll for Topwear for DEF 50% - Improves weapon def. on topwear.\nSuccess rate:50%, weapon def. +5, magic def. +4
            2048000, //  Scroll for Earring for INT 50% - Improves INT on ear accessory.\nSuccess rate:50%, magic attack +5, INT+3, magic def. +2
            2048001, //  Scroll for Helmet for DEF 50% - Improves helmet def.\nSuccess Rate:50%, weapon def.+5, magic def.+4
            2048002, //  Scroll for Helmet for HP 50% - Improves MaxHP on hats.\nSuccess rate:50%, MaxHP+35
            2048003, //  Seasoned Frog Eggs and Mushrooms - A stinky dish made with Cursed Frog Eggs. Increases 400 HP if eaten.
            2048004, //  Bloody Mushroom Wine - A non-alcoholic wine made by the Witch using Cursed Cat Spittle. Increases 200 MP when consumed.
            2048005, //  Slimy Canape - A creepy and slimy Canape that the Witch has made for you. Increases Weapon ATT and Weapon DEF +15 for 15 minutes.
            2048006, //  Zingy Kabab - A suspiciously sharp-tasting Kabab that the Witch has made for you.  Increases Magic ATT And Magic DEF +15 for 15 minutes.
            2048007, //  Swamp Wrap - The Witch's wrap dish that fills your mouth with the aroma of the swamp with each bite. Increases Weapon ATT and Weapon DEF +25 for 15 minutes.
            2048008, //  Rough Leather Steak - A rough leather steak that the Witch has carefully grilled for you. Increases Magic ATT And Magic Defense +25 for 15 minutes.
            2048009, //  Witch's Special Stew - The Witch's special stew that gives off a sour stink. Increases Weapon and Magic ATT +40, Weapons and Magic DEF +100, and Speed and Jump +15 for 15 minutes.
            2048010, //  Normal Witch Scroll - A scroll that, depending on chance, will either improve or worsen the Talking Witch Hat or Broomstick obtained from the Witch.
            2048011, //  Witch's Belt Scroll - A scroll that, depending on chance, will either improve or worsen the Witch's Belts obtained from the Witch.
            2048012, //  Turkey Leg - A huge roasted turkey leg. It's so big that it seems like it would take all day to finish eating.\n[Gives +30 Physical Attack for 3 minutes]
            2048013, //  Special Rien Red Potion - A special bottle of potion consisting of herbs that only grow in Rien. \nRecovers HP +100.
            2049000, //  Special Rien Blue Potion - A special bottle of potion consisting of herbs that only grow in Rien. \nRecovers MP +50.
            2049001, //  Angelic Steps - Allows one to move fast. \nWill increase your speed for 10 minutes.
            2049002, //  Angel Apple - A red, ripe apple.\nRecovers 200 HP.
            2049003, //  Angel Lemon - A very sour fruit.\nRecovers 200 MP.
            2049100, //  Aran Paper Box - A paper box containing a special present commemorating the Aran release. It is well-packed, so you may need to #cdouble-click# to open.
            2049101, //  King Pepe Warrior Weapon Box - King Pepe's box containing a Warrior weapon.
            2049102, //  King Pepe Magician Weapon Box - King Pepe's box containing a Magician weapon.
            2049103, //  King Pepe Bowman Weapon Box - King Pepe's box containing a Bowman weapon.
            2049104, //  King Pepe Thief Weapon Box - King Pepe's box containing a Thief weapon.
            2049105, //  King Pepe Pirate Weapon Box - King Pepe's box containing a Pirate weapon.
            2049106, //  King Pepe Warrior Armor Box - King Pepe's box containing armor for Warriors.
            2049107, //  King Pepe Magician Armor Box - King Pepe's box containing armor for Magicians.
            2049108, //  King Pepe Bowman Armor Box - King Pepe's box containing armor for Bowmen.
            2049109, //  King Pepe Thief Armor Box - King Pepe's box containing armor for Thieves.
            2049110, //  King Pepe Pirate Armor Box - King Pepe's box containing armor for Pirates.
            2049112, //  King Pepe Warrior Box - King Pepe's box containing an equipment item for Warriors.
            2049113, //  King Pepe Magician Box - King Pepe's box containing an equipment item for Magicians.
            2049114, //  King Pepe Bowman Box - King Pepe's box containing an equipment item for Bowmen.
            2049200, //  King Pepe Thief Box - King Pepe's box containing an equipment item for Thieves.
            2049201, //  King Pepe Pirate Box - King Pepe's box containing an equipment item for Pirates.
            2049202, //  Pharaoh's Blessing Lv. 1 - 100% Damage, Attack Speed +1
            2049203, //  Pharaoh's Blessing Lv. 2 - 200% Damage, Attack Speed +2
            2049204, //  Pharaoh's Blessing Lv. 3 - 300% Damage, Attack Speed +3
            2049205, //  Pharaoh's Blessing Lv. 4 - 400% Damage, Attack Speed +4
            2049206, //  Pharaoh's Treasure Chest - A treasure chest containing Pharaoh's treasured items. Double-click to open.
            2049207, //  Bingo Gift Box - A surprise gift box received after completing a line on the Bingo Board. Open it to discover what's inside!
            2049208, //  Subway Lost and Found - A Lost and Found box accidentally ingested by Bubbling. Double-click to open.
            2049209, //  Power Buff Lv. 1 - 100% Damage, Attack Speed +1
            2049210, //  Power Buff Lv. 2 - 200% Damage, Attack Speed +2
            2049211, //  Pharaoh's Treasure Chest - A treasure chest containing Pharaoh's precious items. Double-click to open.
            2050000, //  Rien Teleport Ticket - A teleport ticket given by the Maple Admin. It will immediately teleport you to Rien.
            2050001, //  King Pepe's 60% Scroll for One-handed Sword Attacks - Improves the Attack strength of King Pepe's Cutlass.\nSuccess Rate: 60%, Weapon Attack +2, STR +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2050002, //  King Pepe's 60% Scroll for One-handed Axe Attacks - Improves the Attack strength of King Pepe's Danker.\nSuccess Rate: 60%, Weapon Attack +2, STR +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2050003, //  King Pepe's 60% Scroll for One-handed BW Attacks - Improves the Attack strength of King Pepe's Heavy Hammer.\nSuccess Rate: 60%, Weapon Attack +2, STR +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2050004, //  King Pepe's 60% Scroll for Dagger Attacks - Improves the Attack strength of King Pepe's Gephart.\nSuccess Rate: 60%, Weapon Attack +2, Luck +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2050005, //  King Pepe's 60% Scroll for Wand Magic Attacks - Improves the Magic Attack strength of King Pepe's Wizard Wand.\nSuccess Rate: 60%, Magic Attack +2, Intelligence +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2050006, //  King Pepe's 60% Scroll for Staff Magic Attacks - Improves the Magic Attacks strength of King Pepe's Petal Staff.\nSuccess Rate: 60%, Magic Attack +2, Intelligence +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2050098, //  King Pepe's 60% Scroll for Two-handed Sword Attacks - Improves the Attack strength of King Pepe's Highlander.\nSuccess Rate: 60%, Weapon Attack +2, Strength +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2050099, //  King Pepe's 60% Scroll for Two-handed Axe Attacks - Improves the Attack strength of King Pepe's Niam.\nSuccess Rate: 60%, Weapon Attack +2, Strength +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2060000, //  King Pepe's 60% Scroll for Two-handed BW Attacks - Improves the Attack strength of King Pepe's Big Hammer.\nSuccess Rate: 60%, Weapon Attack +2, Strength +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2060001, //  King Pepe's 60% Scroll for Spear Attacks - Improves the Attack strength of King Pepe's Nakamaki.\nSuccess Rate: 60%, Weapon Attack +2, Strength +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2060002, //  King Pepe's 60% Scroll for Polearm Attacks - Improves the Attack strength of King Pepe's Axe Polearm.\nSuccess Rate: 60%, Weapon Attack +2, Strength +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2060003, //  King Pepe's 60% Scroll for Bow Attacks - Improves the Attack strength of King Pepe's Red Viper.\nSuccess Rate: 60%, Weapon Attack +2, Accuracy +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2060004, //  King Pepe's 60% Scroll for Crossbow Attacks - Improves the Attack strength of King Pepe's Eagle Crow.\nSuccess Rate: 60%, Weapon Attack +2, Accuracy +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2060005, //  King Pepe's 60% Scroll for Thief Attacks - Improves the Attack strength of King Pepe's Dark Guardian.\nSuccess Rate: 60%, Weapon Attack +2, Accuracy +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2060006, //  King Pepe's 60% Scroll for Knuckle Attacks - Improves the Attack strength of King Pepe's Silver Maden.\nSuccess Rate: 60%, Weapon Attack +2, Strength +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2061000, //  King Pepe's 60% Scroll for Gun Attacks - Improves the Attack strength of King Pepe's Shooting Star.\nSuccess Rate: 60%, Weapon Attack +2, Accuracy +1. The success rate of this scroll can be enhanced by Vega's Spell.
            2061001, //  King Pepe's 100% Scroll for Weapons - Improves or decreases the effectiveness of King Pepe weapons.\nSuccess Rate: 100%
            2061002, //  Scroll for Shoes for ATT - Improves attack on shoes.\nSuccess rate: 100%. Weapon Attack +1
            2061003, //  Scroll for Shoes for ATT - Improves attack on gloves.\nSuccess rate: 60%. Weapon Attack +2. The success rate of this scroll can be enhanced by Vega's Spell.
            2061004, //  Scroll for Shoes for ATT - Improves attack on shoes.\nSuccess rate: 10%, Weapon Attack +3. The success rate of this scroll can be enhanced by Vega's Spell.
            2070000, //  Scroll for Knuckler for Attack 100% - Improves attack on Knucklers.\nSuccess rate: 100%. Weapon Attack +2, STR +1
            2070001, //  Scroll for Knuckler for Attack 50% - Improves attack on Knucklers.\nSuccess rate: 50%. Weapon Attack +5, STR +3,  DEX +1
            2070002, //  Scroll for Gun for Attack 100% - Improves attack on Guns.\nSuccess rate: 100%. Weapon Attack +2, Accuracy +1
            2070003, //  Scroll for Gun for Attack 50% - Improves attack on Guns.\nSuccess rate: 50%. Weapon Attack +5, LUK +1, DEX +1
            2070004, //  White Elixir - A legendary potion. \nRecovers 50% HP and MP and cancels abnormal statuses. It can recover all abnormal statuses.
            2070005, //  Dynamite Drink EX - A drink with "Energy Boost" written on the label. Consuming this drink will grant ATT +25, MP +30, DEF +30, and Jump +10 for 30 minutes.
            2070006, //  Energy Drink - Consuming this drink will increase MAX HP for a set amount of time.
            2070007, //  Dark Scroll for Accessory for STR 70%   - Improves STR on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 70%, STR+2\nIf failed, the item will be destroyed at a 50% rate.
            2070008, //  Dark Scroll for Accessory for STR 30% - Improves STR on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 30%, STR+3\nIf failed, the item will be destroyed at a 50% rate.
            2070009, //  Dark Scroll for Accessory for DEX 70% - Improves DEX on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 70%, DEX+2\nIf failed, the item will be destroyed at a 50% rate.
            2070010, //  Dark Scroll for Accessory for DEX 30% - Improves DEX on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 30%, DEX+3\nIf failed, the item will be destroyed at a 50% rate.
            2070011, //  Dark Scroll for Accessory for INT 70% - Improves INT on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 70%. INT+2\nIf failed, the item will be destroyed at a 50% rate.
            2070012, //  Dark Scroll for Accessory for INT 30% - Improves INT on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 30%. INT+3\nIf failed, the item will be destroyed at a 50% rate.
            2070013, //  Dark Scroll for Accessory for LUK 70% - Improves LUK on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 70%, LUK+2\nIf failed, the item will be destroyed at a 50% rate.
            2070014, //  Dark Scroll for Accessory for LUK 30% - Improves LUK on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 30%, LUK+3\nIf failed, the item will be destroyed at a 50% rate.
            2070015, //  Dark Scroll for Accessory for HP 70% - Improves MaxHP on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 70%, MaxHP+10\nIf failed, the item will be destroyed at a 50% rate.
            2070016, //  Dark Scroll for Accessory for HP 30% - Improves MaxHP on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 30%, MaxHP+30\nIf failed, the item will be destroyed at a 50% rate.
            2070018, //  Dark Scroll for Accessory for MP 70% - Improves MaxMP on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 70%, MaxMP+10\nIf failed, the item will be destroyed at a 50% rate.
            2083000, //  Dark Scroll for Accessory for MP 30% - Improves MaxMP on Accessories (Pendants, Belts, Rings).\nSuccess Rate: 30%, MaxMP+30\nIf failed, the item will be destroyed at a 50% rate.
            2084000, //  Scroll for Polearm for Accuracy 65% - Improves Accuracy on Polearms.\nSuccess Rate: 65%, Accuracy+3, DEX+2, Weapon Attack+1
            2100000, //  Scroll for Polearm for Accuracy 15% - Improves Accuracy on Polearms.\nSuccess Rate: 15%, Accuracy+5, DEX+3, Weapon Attack+3
            2100001, //  Scroll for Spears for Accuracy 65% - Improves Accuracy on Spears.\nSuccess Rate: 65%, Accuracy+3, DEX+2, Weapon Attack+1
            2100002, //  Scroll for Spears for Accuracy 15% - Improves Accuracy on Spears.\nSuccess Rate: 15%, Accuracy+5, DEX+3, Weapon Attack+3
            2100003, //  Scroll for Two-Handed BW for Accuracy 65% - Improves Accuracy on Two-Handed Blunt Weapons.\nSuccess Rate: 65%, Accuracy+3, DEX+2, Weapon Attack+1
            2100004, //  Scroll for Two-Handed BW for Accuracy 15% - Improves Accuracy on Two-Handed Blunt Weapons.\nSuccess Rate: 15%, Accuracy+5, DEX+3, Weapon Attack+3
            2100005, //  Scroll for Two-Handed Axe for Accuracy 65% - Improves Accuracy on Two-Handed Axe.\nSuccess Rate: 65%, Accuracy+3, DEX+2, Weapon Attack+1
            2100006, //  Scroll for Two-Handed Axe for Accuracy 15% - Improves Accuracy on Two-Handed Axe.\nSuccess Rate: 15%, Accuracy+5, DEX+3, Weapon Attack+3
            2100007, //  Scroll for Two-Handed Sword for Accuracy 65% - Improves Accuracy on Two-Handed Swords.\nSuccess Rate: 65%, Accuracy+3, DEX+2, Weapon Attack+1
            2100008, //  Scroll for Two-Handed Sword for Accuracy 15% - Improves Accuracy on Two-Handed Swords.\nSuccess Rate: 15%, Accuracy+5, DEX+3, Weapon Attack+3
            2100009, //  Scroll for One-Handed BW for Accuracy 65% - Improves Accuracy on One-Handed Blunt Weapons.\nSuccess Rate: 65%, Accuracy+3, DEX+2, Weapon Attack+1
            2100010, //  Scroll for One-Handed BW for Accuracy 15% - Improves Accuracy on One-Handed Blunt Weapons.\nSuccess Rate: 15%, Accuracy+5, DEX+3, Weapon Attack+3
            2100011, //  Scroll for One-Handed Axe for Accuracy 65% - Improves Accuracy on One-Handed Axe.\nSuccess Rate: 65%, Accuracy+3, DEX+2, Weapon Attack+1
            2100012, //  Scroll for One-Handed Axe for Accuracy 15% - Improves Accuracy on One-Handed Axe.\nSuccess Rate: 15%, Accuracy+5, DEX+3, Weapon Attack+3
            2100013, //  Scroll for One-Handed Sword for Accuracy 65% - Improves Accuracy on One-Handed Swords.\nSuccess Rate: 65%, Accuracy+3, DEX+2, Weapon Attack+1
            2100014, //  Scroll for One-Handed Sword for Accuracy 15% - Improves Accuracy on One-Handed Swords.\nSuccess Rate: 15%, Accuracy+5, DEX+3, Weapon Attack+3
            2100015, //  Scroll for Shield for LUK 65% - Improves LUK on Shields.\nSuccess Rate: 65%, LUK+2
            2100016, //  Scroll for Shield for LUK 15% - Improves LUK on Shields.\nSuccess Rate: 15%, LUK+3
            2100017, //  Scroll for Shield for HP 65% - Improves HP on Shields.\nSuccess Rate: 65%, MaxHP+15
            2100018, //  Scroll for Shield for HP 15% - Improves HP on Shields.\nSuccess Rate: 15%, MaxHP+30
            2100019, //  Scroll for Shield for STR 65% - Improves STR on Shields.\nSuccess Rate: 65%, STR+2
            2100020, //  Scroll for Shield for STR 15% - Improves STR on Shields.\nSuccess Rate: 15%, STR+3
            2100021, //  Scroll for Gloves for HP 65% - Improves HP on Gloves.\nSuccess Rate: 65%, MaxHP+15
            2100022, //  Scroll for Gloves for HP 15% - Improves HP on Gloves.\nSuccess Rate: 15%, MaxHP+30
            2100023, //  Scroll for Bottomwear for Jump 65% - Improves Jump on Bottomwear.\nSuccess Rate: 65%, Jump+2, Avoidability+1
            2100024, //  Scroll for Bottomwear for Jump 15% - Improves Jump on Bottomwear.\nSuccess Rate: 15%, Jump+4, Avoidability+2
            2100025, //  Scroll for Bottomwear for HP 65% - Improves HP on Bottomwear.\nSuccess Rate: 65%, MaxHP+15
            2100026, //  Scroll for Bottomwear for HP 15% - Improves HP on Bottomwear.\nSuccess Rate: 15%, MaxHP+30
            2100027, //  Scroll for Bottomwear for DEX 65% - Improves DEX on Bottomwear.\nSuccess Rate: 65%, DEX+2, Accuracy+1
            2100028, //  Scroll for Bottomwear for DEX 15% - Improves DEX on Bottomwear.\nSuccess Rate: 15%, DEX+3, Accuracy+2, Speed+1
            2100029, //  Scroll for Overall Armor for STR 65% - Improves STR on Overall Armor.\nSuccess Rate: 65%, STR+2, Weapon Attack+1
            2100030, //  Scroll for Overall Armor for STR 15% - Improves STR on Overall Armor.\nSuccess Rate: 15%, STR+5, Weapon Attack+3, MaxHP+5
            2100031, //  Scroll for Topwear for STR 65% - Improves STR on Topwear.\nSuccess Rate 65%, STR+2
            2100032, //  Scroll for Topwear for STR 15% - Improves STR on Topwear.\nSuccess Rate 15%, STR+3
            2100033, //  Scroll for Topwear for HP 65% - Improves HP on Topwear.\nSuccess Rate 65%, MaxHP + 15
            2100034, //  Scroll for Topwear for HP 15% - Improves HP on Topwear.\nSuccess Rate 15%, MaxHP + 30
            2100035, //  Scroll for Topwear for LUK 65% - Improves LUK on Topwear.\nSuccess Rate: 65%, LUK+2
            2100036, //  Scroll for Topwear for LUK 15% - Improves LUK on Topwear.\nSuccess Rate: 15%, LUK+3
            2100037, //  Scroll for Earring for DEX 65% - Improves DEX on Earrings.\nSuccess Rate: 65%, DEX+2
            2100038, //  Scroll for Earring for DEX 15% - Improves DEX on Earrings.\nSuccess Rate: 15%, DEX+3
            2100039, //  Scroll for Earring for LUK 65% - Improves LUK on Earrings.\nSuccess Rate: 65%, LUK+2
            2100040, //  Scroll for Earring for LUK 15% - Improves LUK on Earrings.\nSuccess Rate: 15%, LUK+3
            2100041, //  Scroll for Earring for HP 65% - Improves HP on Earrings.\nSuccess Rate: 65%, MaxHP+15
            2100042, //  Scroll for Earring for HP 15% - Improves HP on Earrings.\nSuccess Rate: 15%, MaxHP+30
            2100043, //  Scroll for Helmet for DEX 65% - Improves DEX on Helmets.\nSuccess Rate 65%, DEX+2
            2100044, //  Scroll for Helmet for DEX 15% - Improves DEX on Helmets.\nSuccess Rate 15%, DEX+3
            2100045, //  Handmade Sandwich  - A sandwich made personally by Anna. Eat it in the morning to guarantee a sense of satisfaction all throughout the day.
            2100046, //  Tasty Milk - Fresh milk produced at the farm. \nRecovers around 100 HP.
            2100047, //  Squeezed Juice - Fresh juice produced at the farm. \nRecovers around 50 MP.
            2100048, //  Rose Scent - As you are surrounded by the fragrant scent of Roses, you get a strange headache and your feet start to feel heavy. Go see Lana before the scent disappears.
            2100049, //  Freesia Scent - As you are surrounded by the refreshing scent of Freesias, you get a strange headache and your feet start to feel heavy. Go see Lana before the scent disappears.
            2100050, //  Lavender Scent - As you are surrounded by the sweet scent of Lavenders, you get a strange headache and your feet start to feel heavy. Go see Lana before the scent disappears.
        };

        #endregion
    }
}
