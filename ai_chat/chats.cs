using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ai_chat
{
    public class chats
    {
        // Response entry class to hold keywords, response and tip
        private class ResponseEntry
        {
            public string[] Keywords { get; set; }
            public string Response { get; set; }
            public string Tip { get; set; }

            public ResponseEntry(string[] keywords, string response, string tip)
            {
                Keywords = keywords;
                Response = response;
                Tip = tip;
            }
        }

        // List to store all response entries
        private List<ResponseEntry> responses = new List<ResponseEntry>();

        // Ignore list (stop words)
        ArrayList ignoring = new ArrayList();

        // Constructor to load responses
        public chats()
        {
            LoadResponses();
            LoadIgnoreWords();
        }

        private void LoadResponses()
        {
            responses.Add(new ResponseEntry(
                new[] { "how are you", "how r you", "how are u" },
                "I'm fully operational and ready to help keep you safe online! Cybersecurity never sleeps.",
                "Remember: staying informed is your best defence against cyber threats."
            ));

            responses.Add(new ResponseEntry(
                new[] { "your purpose", "what do you do", "why are you here", "what is your purpose" },
                "I'm a Cybersecurity Awareness Bot! I can help you understand phishing, safe passwords, malware, and much more!",
                "Knowledge is the first layer of your cybersecurity defence."
            ));

            responses.Add(new ResponseEntry(
                new[] { "what can i ask", "what can you help", "help me", "topics", "what do you know" },
                "You can ask me about:\n  - Password safety\n  - Phishing scams\n  - Safe browsing\n  - Malware and viruses\n  - Privacy protection\n  - Social media safety\n  - Online banking safety\n  - Email security\n  - Two-factor authentication",
                null
            ));

            responses.Add(new ResponseEntry(
                new[] { "password", "passwords", "passphrase", "strong password" },
                "Strong passwords are your first line of defence! Here's what makes a great password:\n" +
                "  - At least 12 characters long\n" +
                "  - Mix of UPPERCASE, lowercase, numbers and symbols\n" +
                "  - Never use personal info like birthdays or names\n" +
                "  - Use a different password for every account\n" +
                "  - Consider using a password manager like Bitwarden",
                "Never share your password — not even with people claiming to be IT support!"
            ));

            responses.Add(new ResponseEntry(
                new[] { "phishing", "phish", "scam email", "fake email", "suspicious email", "email scam" },
                "Phishing is one of the most common attacks!\n" +
                "Warning signs include:\n" +
                "  - Urgent language like 'Act NOW or lose your account!'\n" +
                "  - Spelling mistakes and poor grammar\n" +
                "  - Requests for personal info or passwords\n" +
                "  - Suspicious sender email addresses\n" +
                "  - Links that don't match the company's real website\n" +
                "  When in doubt — DON'T click! Report it instead.",
                "Hover over links BEFORE clicking to see the real destination URL."
            ));

            responses.Add(new ResponseEntry(
                new[] { "safe browsing", "browsing", "website", "internet safety", "online safe", "browse safely" },
                "Staying safe while browsing is essential. Key tips:\n" +
                "  - Always check for HTTPS (padlock icon) before entering info\n" +
                "  - Avoid using public Wi-Fi for banking or shopping\n" +
                "  - Use a reputable browser with security extensions\n" +
                "  - Keep your browser updated to the latest version\n" +
                "  - Use a VPN when on public networks",
                "HTTP sites are NOT secure — never enter passwords on HTTP pages."
            ));

            responses.Add(new ResponseEntry(
                new[] { "malware", "virus", "ransomware", "spyware", "trojan", "worm", "antivirus" },
                "Malware is malicious software designed to harm your device!\n" +
                "Types include:\n" +
                "  - Viruses — spread by infecting files\n" +
                "  - Ransomware — locks your files and demands payment\n" +
                "  - Spyware — secretly monitors your activity\n" +
                "  - Trojans — disguised as legitimate software\n" +
                "  Protection: Install reputable antivirus and keep it updated!",
                "Never download software from untrusted websites or email attachments."
            ));

            responses.Add(new ResponseEntry(
                new[] { "privacy", "personal information", "data protection", "popia", "protect my data" },
                "Protecting your personal information is your right!\n" +
                "  - South Africa's POPIA law protects your personal data\n" +
                "  - Be careful what you share on social media\n" +
                "  - Read privacy policies before signing up for services\n" +
                "  - Use two-factor authentication (2FA) everywhere possible\n" +
                "  - Regularly check what data companies hold about you",
                "The less personal info you share online, the safer you are."
            ));

            responses.Add(new ResponseEntry(
                new[] { "social media", "facebook", "instagram", "tiktok", "twitter", "social engineering" },
                "Social media is a goldmine for cybercriminals! Stay safe by:\n" +
                "  - Setting your profiles to private\n" +
                "  - Never sharing your location in real time\n" +
                "  - Being wary of friend requests from strangers\n" +
                "  - Not posting sensitive info like ID numbers or addresses\n" +
                "  - Reporting suspicious accounts or messages immediately",
                "Cybercriminals use social media to build profiles and target victims."
            ));

            responses.Add(new ResponseEntry(
                new[] { "banking", "online banking", "internet banking", "bank", "credit card", "otp" },
                "Online banking safety is critical. Follow these rules:\n" +
                "  - Only use your bank's official app or website\n" +
                "  - Never click links in emails claiming to be from your bank\n" +
                "  - Never share your OTP — your bank will NEVER ask for it\n" +
                "  - Check your statements regularly for suspicious transactions\n" +
                "  - Log out completely after every banking session",
                "Your bank will NEVER ask for your PIN, password, or OTP via email or phone."
            ));

            responses.Add(new ResponseEntry(
                new[] { "email", "email security", "spam", "attachment" },
                "Email is the number 1 attack vector for cybercriminals!\n" +
                "  - Never open attachments from unknown senders\n" +
                "  - Be suspicious of unexpected emails, even from known contacts\n" +
                "  - Enable spam filtering on your email account\n" +
                "  - Use a separate email for shopping and newsletters\n" +
                "  - Enable 2FA on your email — it's your digital master key!",
                "If an email feels off — trust your instincts and verify via phone first."
            ));

            responses.Add(new ResponseEntry(
                new[] { "two factor", "2fa", "two-factor", "authentication", "mfa" },
                "Two-Factor Authentication (2FA) adds a crucial extra layer of security!\n" +
                "  - Even if your password is stolen, 2FA keeps attackers out\n" +
                "  - Use an authenticator app like Google Authenticator or Authy\n" +
                "  - Enable 2FA on email, banking, and social media accounts first\n" +
                "  - Never share your 2FA code with anyone — ever!",
                "App-based 2FA is more secure than SMS-based 2FA."
            ));
        }

        private void LoadIgnoreWords()
        {
            ignoring.Add("what"); 
            ignoring.Add("is");
            ignoring.Add("about");
            ignoring.Add("a"); 
            ignoring.Add("above");
            ignoring.Add("across");
            ignoring.Add("after"); 
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
            ignoring.Add("an");
            ignoring.Add("and");
            ignoring.Add("another");
            ignoring.Add("any");
            ignoring.Add("are");
            ignoring.Add("as");
            ignoring.Add("at");
            ignoring.Add("be");
            ignoring.Add("because");
            ignoring.Add("been");
            ignoring.Add("before");
            ignoring.Add("being");
            ignoring.Add("both");
            ignoring.Add("but");
            ignoring.Add("by");
            ignoring.Add("can");
            ignoring.Add("could");
            ignoring.Add("did");
            ignoring.Add("do"); 
            ignoring.Add("does");
            ignoring.Add("done");
            ignoring.Add("down");
            ignoring.Add("each");
            ignoring.Add("even");
            ignoring.Add("ever");
            ignoring.Add("every");
            ignoring.Add("for");
            ignoring.Add("from");
            ignoring.Add("had");
            ignoring.Add("has"); 
            ignoring.Add("have");
            ignoring.Add("he");
            ignoring.Add("her");
            ignoring.Add("here"); 
            ignoring.Add("him");
            ignoring.Add("his");
            ignoring.Add("how"); 
            ignoring.Add("i");
            ignoring.Add("if");
            ignoring.Add("in");
            ignoring.Add("into");
            ignoring.Add("it");
            ignoring.Add("its");
            ignoring.Add("me");
            ignoring.Add("more");
            ignoring.Add("my");
            ignoring.Add("no");
            ignoring.Add("not");
            ignoring.Add("now");
            ignoring.Add("of");
            ignoring.Add("on");
            ignoring.Add("or");
            ignoring.Add("our");
            ignoring.Add("out");
            ignoring.Add("own"); 
            ignoring.Add("please");
            ignoring.Add("so");
            ignoring.Add("some");
            ignoring.Add("than");
            ignoring.Add("that"); 
            ignoring.Add("the"); 
            ignoring.Add("their");
            ignoring.Add("them");
            ignoring.Add("then");
            ignoring.Add("there");
            ignoring.Add("these");
            ignoring.Add("they");
            ignoring.Add("this");
            ignoring.Add("those");
            ignoring.Add("though");
            ignoring.Add("through");
            ignoring.Add("to"); 
            ignoring.Add("too");
            ignoring.Add("up");
            ignoring.Add("us");
            ignoring.Add("was");
            ignoring.Add("we");
            ignoring.Add("were");
            ignoring.Add("when");
            ignoring.Add("where");
            ignoring.Add("which");
            ignoring.Add("while");
            ignoring.Add("who");
            ignoring.Add("will");
            ignoring.Add("with");
            ignoring.Add("would");
            ignoring.Add("you");
            ignoring.Add("your");
            ignoring.Add("yourself");
        }

        // Method to chat
        public void ai_chats(string name)
        {
            string asking = string.Empty;
            do
            {
                Console.Write(name + " : ");
                asking = Console.ReadLine();

            } while (end_chat(asking));
        }

        // Method to check if exit or chatting
        private Boolean end_chat(string question)
        {
            if (question.ToLower() == "exit")
            {
                Console.WriteLine("ChatBot: Bye..");
                return false;
            }
            else
            {
                string lower = question.ToLower().Trim();
                bool found = false;

                // First try to match full phrases (multi-word keywords)
                foreach (ResponseEntry entry in responses)
                {
                    foreach (string keyword in entry.Keywords)
                    {
                        if (lower.Contains(keyword))
                        {
                            Console.WriteLine("\nChatbot: " + entry.Response);
                            if (!string.IsNullOrEmpty(entry.Tip))
                            {
                                Console.WriteLine("\nTip: " + entry.Tip);
                            }
                            Console.WriteLine();
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }

                // If no phrase matched, fall back to word-by-word search
                if (!found)
                {
                    string[] find_words = question.Split(' ');

                    foreach (string word in find_words)
                    {
                        if (ignoring.Contains(word.ToLower())) continue;

                        foreach (ResponseEntry entry in responses)
                        {
                            foreach (string keyword in entry.Keywords)
                            {
                                if (keyword.Contains(word.ToLower()))
                                {
                                    Console.WriteLine("\nChatbot: " + entry.Response);
                                    if (!string.IsNullOrEmpty(entry.Tip))
                                    {
                                        Console.WriteLine("\nTip: " + entry.Tip);
                                    }
                                    Console.WriteLine();
                                    found = true;
                                    break;
                                }
                            }
                            if (found) break;
                        }
                        if (found) break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("ChatBot: I don't understand, please try again. Type 'help' to see what I can help with.");
                }

                return true;
            }
        }
    }
}