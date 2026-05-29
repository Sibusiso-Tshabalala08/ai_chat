using System.Collections;

namespace demo
{// start of namespace

    public class respond
    {// start of class

        // Constructor — called when a new respond object is created in MainWindow
        // Immediately loads all answers and ignore words into the provided ArrayLists
        public respond(ArrayList reply, ArrayList ignore)
        {// start of constructor

            // Load all chatbot answers into the reply list
            answers(reply);

            // Load all words to ignore during keyword matching
            words(ignore);

        }// end of constructor


        // -----------------------------------------------------------------------
        // WORDS METHOD
        // Populates the ignore list with common filler/stopwords
        // These words are skipped during keyword matching in ai_check
        // so the bot focuses only on meaningful content words
        // -----------------------------------------------------------------------
        private void words(ArrayList ignoring)
        {// start of words method

            ignoring.Add("a");
            ignoring.Add("about");
            ignoring.Add("above");
            ignoring.Add("across");
            ignoring.Add("after");
            ignoring.Add("afterwards");
            ignoring.Add("again");
            ignoring.Add("against");
            ignoring.Add("all");
            ignoring.Add("almost");
            ignoring.Add("alone");
            ignoring.Add("along");
            ignoring.Add("already");
            ignoring.Add("also");
            ignoring.Add("although");
            ignoring.Add("always");
            ignoring.Add("am");
            ignoring.Add("among");
            ignoring.Add("amongst");
            ignoring.Add("amount");
            ignoring.Add("an");
            ignoring.Add("and");
            ignoring.Add("another");
            ignoring.Add("any");
            ignoring.Add("anyhow");
            ignoring.Add("anyone");
            ignoring.Add("anything");
            ignoring.Add("anyway");
            ignoring.Add("anywhere");
            ignoring.Add("are");
            ignoring.Add("around");
            ignoring.Add("as");
            ignoring.Add("at");
            ignoring.Add("back");
            ignoring.Add("be");
            ignoring.Add("became");
            ignoring.Add("because");
            ignoring.Add("become");
            ignoring.Add("becomes");
            ignoring.Add("becoming");
            ignoring.Add("been");
            ignoring.Add("before");
            ignoring.Add("beforehand");
            ignoring.Add("behind");
            ignoring.Add("being");
            ignoring.Add("below");
            ignoring.Add("beside");
            ignoring.Add("besides");
            ignoring.Add("between");
            ignoring.Add("beyond");
            ignoring.Add("both");
            ignoring.Add("but");
            ignoring.Add("by");
            ignoring.Add("can");
            ignoring.Add("cannot");
            ignoring.Add("could");
            ignoring.Add("did");
            ignoring.Add("do");
            ignoring.Add("does");
            ignoring.Add("doing");
            ignoring.Add("done");
            ignoring.Add("down");
            ignoring.Add("during");
            ignoring.Add("each");
            ignoring.Add("either");
            ignoring.Add("else");
            ignoring.Add("elsewhere");
            ignoring.Add("enough");
            ignoring.Add("etc");
            ignoring.Add("even");
            ignoring.Add("ever");
            ignoring.Add("every");
            ignoring.Add("everyone");
            ignoring.Add("everything");
            ignoring.Add("everywhere");
            ignoring.Add("except");
            ignoring.Add("few");
            ignoring.Add("first");
            ignoring.Add("for");
            ignoring.Add("former");
            ignoring.Add("formerly");
            ignoring.Add("from");
            ignoring.Add("further");
            ignoring.Add("had");
            ignoring.Add("has");
            ignoring.Add("have");
            ignoring.Add("having");
            ignoring.Add("he");
            ignoring.Add("hence");
            ignoring.Add("her");
            ignoring.Add("here");
            ignoring.Add("hereafter");
            ignoring.Add("hereby");
            ignoring.Add("herein");
            ignoring.Add("hereupon");
            ignoring.Add("hers");
            ignoring.Add("herself");
            ignoring.Add("him");
            ignoring.Add("himself");
            ignoring.Add("his");
            ignoring.Add("how");
            ignoring.Add("however");
            ignoring.Add("i");
            ignoring.Add("if");
            ignoring.Add("in");
            ignoring.Add("indeed");
            ignoring.Add("inside");
            ignoring.Add("instead");
            ignoring.Add("into");
            ignoring.Add("is");
            ignoring.Add("it");
            ignoring.Add("its");
            ignoring.Add("itself");
            ignoring.Add("last");
            ignoring.Add("later");
            ignoring.Add("latter");
            ignoring.Add("latterly");
            ignoring.Add("least");
            ignoring.Add("less");
            ignoring.Add("lot");
            ignoring.Add("many");
            ignoring.Add("may");
            ignoring.Add("me");
            ignoring.Add("meanwhile");
            ignoring.Add("might");
            ignoring.Add("more");
            ignoring.Add("moreover");
            ignoring.Add("most");
            ignoring.Add("mostly");
            ignoring.Add("much");
            ignoring.Add("must");
            ignoring.Add("my");
            ignoring.Add("myself");
            ignoring.Add("name");
            ignoring.Add("namely");
            ignoring.Add("neither");
            ignoring.Add("never");
            ignoring.Add("nevertheless");
            ignoring.Add("next");
            ignoring.Add("no");
            ignoring.Add("nobody");
            ignoring.Add("none");
            ignoring.Add("noone");
            ignoring.Add("nor");
            ignoring.Add("not");
            ignoring.Add("nothing");
            ignoring.Add("now");
            ignoring.Add("nowhere");
            ignoring.Add("of");
            ignoring.Add("off");
            ignoring.Add("often");
            ignoring.Add("on");
            ignoring.Add("once");
            ignoring.Add("one");
            ignoring.Add("only");
            ignoring.Add("or");
            ignoring.Add("other");
            ignoring.Add("others");
            ignoring.Add("otherwise");
            ignoring.Add("ought");
            ignoring.Add("our");
            ignoring.Add("ours");
            ignoring.Add("ourselves");
            ignoring.Add("out");
            ignoring.Add("outside");
            ignoring.Add("over");
            ignoring.Add("own");
            ignoring.Add("part");
            ignoring.Add("per");
            ignoring.Add("perhaps");
            ignoring.Add("please");
            ignoring.Add("put");
            ignoring.Add("rather");
            ignoring.Add("re");
            ignoring.Add("same");
            ignoring.Add("see");
            ignoring.Add("seem");
            ignoring.Add("seemed");
            ignoring.Add("seeming");
            ignoring.Add("seems");
            ignoring.Add("several");
            ignoring.Add("she");
            ignoring.Add("should");
            ignoring.Add("show");
            ignoring.Add("side");
            ignoring.Add("since");
            ignoring.Add("so");
            ignoring.Add("some");
            ignoring.Add("somehow");
            ignoring.Add("someone");
            ignoring.Add("something");
            ignoring.Add("sometime");
            ignoring.Add("sometimes");
            ignoring.Add("somewhere");
            ignoring.Add("still");
            ignoring.Add("such");
            ignoring.Add("take");
            ignoring.Add("than");
            ignoring.Add("that");
            ignoring.Add("the");
            ignoring.Add("their");
            ignoring.Add("theirs");
            ignoring.Add("them");
            ignoring.Add("themselves");
            ignoring.Add("then");
            ignoring.Add("thence");
            ignoring.Add("there");
            ignoring.Add("thereafter");
            ignoring.Add("thereby");
            ignoring.Add("therefore");
            ignoring.Add("therein");
            ignoring.Add("thereupon");
            ignoring.Add("these");
            ignoring.Add("they");
            ignoring.Add("this");
            ignoring.Add("those");
            ignoring.Add("though");
            ignoring.Add("through");
            ignoring.Add("throughout");
            ignoring.Add("thru");
            ignoring.Add("thus");
            ignoring.Add("to");
            ignoring.Add("together");
            ignoring.Add("too");
            ignoring.Add("toward");
            ignoring.Add("towards");
            ignoring.Add("under");
            ignoring.Add("unless");
            ignoring.Add("until");
            ignoring.Add("up");
            ignoring.Add("upon");
            ignoring.Add("us");
            ignoring.Add("used");
            ignoring.Add("very");
            ignoring.Add("via");
            ignoring.Add("was");
            ignoring.Add("we");
            ignoring.Add("well");
            ignoring.Add("were");
            ignoring.Add("what");
            ignoring.Add("whatever");
            ignoring.Add("when");
            ignoring.Add("whence");
            ignoring.Add("whenever");
            ignoring.Add("where");
            ignoring.Add("whereafter");
            ignoring.Add("whereas");
            ignoring.Add("whereby");
            ignoring.Add("wherein");
            ignoring.Add("whereupon");
            ignoring.Add("wherever");
            ignoring.Add("whether");
            ignoring.Add("which");
            ignoring.Add("while");
            ignoring.Add("whither");
            ignoring.Add("who");
            ignoring.Add("whoever");
            ignoring.Add("whole");
            ignoring.Add("whom");
            ignoring.Add("whose");
            ignoring.Add("why");
            ignoring.Add("will");
            ignoring.Add("with");
            ignoring.Add("within");
            ignoring.Add("without");
            ignoring.Add("would");
            ignoring.Add("yes");
            ignoring.Add("yet");
            ignoring.Add("hey");
            ignoring.Add("you");
            ignoring.Add("your");
            ignoring.Add("yours");
            ignoring.Add("yourself");
            ignoring.Add("yourselves");

        }// end of words method


        // -----------------------------------------------------------------------
        // ANSWERS METHOD
        // Populates the reply ArrayList with all possible chatbot responses
        // Each entry starts with a keyword — ai_check matches user input against these
        // Format: "keyword response text here"
        // -----------------------------------------------------------------------
        public void answers(ArrayList add_answers)
        {// start of answers method


            // --- GREETINGS ---
            // Triggered when user types words like "greeting", "how are you"
            add_answers.Add("greeting i'm doing well, thanks for asking! how are you doing today?");
            add_answers.Add("greeting i'm great today, thanks for asking! how can i help you today?");
            add_answers.Add("greeting doing good! hope you are also doing well today?");

            // Added "hello" and "hi" triggers so users can greet naturally
            add_answers.Add("hi hey there! how can I help you with cybersecurity today?");
            add_answers.Add("hi great to hear from you! what cybersecurity question do you have?");
            add_answers.Add("hi hello! i am here to help with all things cybersecurity.");
            add_answers.Add("hi hey! feel free to ask me anything about staying safe online.");


            // --- PURPOSE ---
            // Triggered when user asks what the bot does or its purpose
            add_answers.Add("purpose my purpose is to educate you on how to stay safe online and guide your cybersecurity questions.");
            add_answers.Add("purpose i help users understand online safety and digital protection.");
            add_answers.Add("purpose i assist with cybersecurity awareness and safety guidance.");


            // --- CYBERSECURITY ---
            // Triggered when user mentions cybersecurity generally
            add_answers.Add("cybersecurity cybersecurity is about protecting systems and networks from digital threats.");
            add_answers.Add("cybersecurity it involves protecting devices and online accounts from attacks.");
            add_answers.Add("cybersecurity it focuses on securing digital information and systems.");


            // --- PHISHING ---
            // Triggered when user asks about phishing attacks or scam emails
            add_answers.Add("phishing phishing is a scam where attackers pretend to be trusted sources to steal information.");
            add_answers.Add("phishing it uses fake messages or websites to trick users into revealing sensitive data.");
            add_answers.Add("phishing attackers use deception to make users believe they are legitimate.");


            // --- FIREWALL ---
            // Triggered when user asks about firewalls and network protection
            add_answers.Add("firwall a firewall controls network traffic based on security rules.");
            add_answers.Add("firwall it helps block unwanted access to your device or network.");
            add_answers.Add("firwall it acts as a protective barrier between trusted and untrusted networks.");


            // --- PASSWORD ---
            // Triggered when user asks about passwords and account security
            add_answers.Add("password a password is used to secure access to your accounts or devices.");
            add_answers.Add("password it should be strong, long and not easy to guess.");
            add_answers.Add("password avoid using personal details when creating one.");


            // --- HACKED ACCOUNT ---
            // Triggered when user mentions their account being hacked or compromised
            add_answers.Add("hacked account immediately secure your account and log out of all devices.");
            add_answers.Add("hacked account contact support if your account has been compromised.");
            add_answers.Add("hacked account enable extra security like two-factor authentication.");


            // --- FRAUD ---
            // Triggered when user asks about financial fraud or suspicious activity
            add_answers.Add("fraud contact your bank immediately if fraud is detected.");
            add_answers.Add("fraud report suspicious financial activity to the authorities.");
            add_answers.Add("fraud monitor your accounts for unusual activity.");


            // --- MALICIOUS CHATBOT ---
            // Triggered when user asks about fake or dangerous chatbots
            add_answers.Add("malicious bot malicious bots often create urgency to trick users.");
            add_answers.Add("malicious bot fake chatbots may ask for sensitive information.");
            add_answers.Add("malicious bot be cautious if a bot pressures you for personal data.");


            // --- VPN ---
            // Triggered when user asks about VPNs and online privacy
            add_answers.Add("vpn a vpn helps protect your privacy on public wi-fi.");
            add_answers.Add("vpn it encrypts your internet traffic for safety.");
            add_answers.Add("vpn it improves security when using public networks.");


            // --- ENCRYPTION ---
            // Triggered when user asks about encryption and data protection
            add_answers.Add("encryption encryption converts data into a coded format that only authorized parties can read.");
            add_answers.Add("encryption it protects sensitive information from being accessed by unauthorized users.");
            add_answers.Add("encryption always ensure websites use HTTPS as it means your connection is encrypted.");


            // --- RANSOMWARE ---
            // Triggered when user asks about ransomware attacks
            add_answers.Add("ransomware ransomware is malware that locks your files and demands payment to restore them.");
            add_answers.Add("ransomware always back up your files regularly to protect against ransomware attacks.");
            add_answers.Add("ransomware never pay the ransom as it does not guarantee your files will be returned.");


            // --- TWO-FACTOR AUTHENTICATION ---
            // Triggered when user asks about 2FA or two-factor authentication
            add_answers.Add("two-factor authentication two-factor authentication adds an extra layer of security beyond just a password.");
            add_answers.Add("two-factor authentication it requires a second verification step such as a code sent to your phone.");
            add_answers.Add("two-factor authentication enabling two-factor authentication greatly reduces the risk of being hacked.");


            // --- DATA BREACH ---
            // Triggered when user asks about data breaches or stolen data
            add_answers.Add("data breach a data breach is when sensitive information is accessed or stolen without permission.");
            add_answers.Add("data breach change your passwords immediately if you think your data has been breached.");
            add_answers.Add("data breach monitor your accounts for unusual activity after any known data breach.");


            // --- SOCIAL ENGINEERING ---
            // Triggered when user asks about manipulation-based attacks
            add_answers.Add("social engineering social engineering tricks people into giving up confidential information.");
            add_answers.Add("social engineering attackers use psychological manipulation to gain your trust and steal information.");
            add_answers.Add("social engineering always verify the identity of anyone requesting your sensitive information.");


            // ---- PRIVACY ---
            // Triggered when user asks about online privacy and data protection
            add_answers.Add("privacy protect your privacy by limiting the personal information you share online.");
            add_answers.Add("privacy review app permissions regularly to ensure apps only access what they truly need.");
            add_answers.Add("privacy adjust your social media privacy settings to control who can see your information.");


            // --- SENTIMENT DETECTION ---
            // These responses detect the emotional tone of the user's message
            // and respond with empathy before offering help

            // Frustrated user responses
            add_answers.Add("frustrated i understand you're frustrated. let's work through the issue step by step.");
            add_answers.Add("frustrated it's okay to feel frustrated when things aren't working. i'm here to help.");
            add_answers.Add("frustrated take a breath, we'll fix this together.");

            // Confused user responses
            add_answers.Add("confusion that's okay, confusion is normal. i'll explain it clearly for you.");
            add_answers.Add("confusion let me break it down step by step so it makes sense.");
            add_answers.Add("confusion no worries, i'll help you understand it better.");

            // Worried user responses
            add_answers.Add("worried it's okay to feel worried. i'm here to help you stay safe online.");
            add_answers.Add("worried don't panic, most cybersecurity issues can be fixed quickly.");
            add_answers.Add("worried i understand your concern. let's make sure your information is safe.");

            // Happy user responses
            add_answers.Add("happy that's great to hear! i'm glad things are going well.");
            add_answers.Add("happy awesome! positivity is always good.");
            add_answers.Add("happy i'm happy for you! let me know if you need anything.");

            // Sad user responses
            add_answers.Add("sad i'm sorry you're feeling this way. i'm here for you.");
            add_answers.Add("sad that sounds tough, take things one step at a time.");
            add_answers.Add("sad i hope things improve soon. you can talk to me anytime.");

            // Angry user responses
            add_answers.Add("angry i understand you're angry. let's try solve the issue together.");
            add_answers.Add("angry it's okay to feel angry, but i'll help you fix the problem.");
            add_answers.Add("angry take your time, i'm here to help you sort it out.");

            // Triggered when user expresses curiosity about a topic
            add_answers.Add("curious that's great that you're curious! curiosity is the first step to staying safe online.");
            add_answers.Add("curious i love the curiosity! let me share some useful cybersecurity knowledge with you.");
            add_answers.Add("curious being curious about cybersecurity is always a good thing, let me help you learn more.");


        }// end of answers method


    }// end of class
}// end of namespace