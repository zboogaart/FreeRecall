# FreeRecall Instructions
*Author: Zach Boogaart
Last Edited: 04/14/2022*
## Summary
This is the FreeRecall task. The goal of the project is to determine how well people can remember a set list of words. The study is performed over 10 sessions on unique days for 2 hours each session.

**First Week Task:**
- Participants will receive a shuffled list of the word pool assigned to them. They will then have words shown to them in increasing size per the step-up function.
  - If a participant starts with 3 words, gets that right, they will step up 5 words to now have an 8-word list with the first 3 words being the 3 from the previous list.
  - If they get the 8-word list incorrect 6 times, they will step down to 7 words that are from the 8-word list. They would then have 5 attempts to get that list right. If they do get it right, it will add the word that was taken away back into the list (so now 8 again), and then they will have 6 attempts to get that right to move on.
- Words pull from the previous day (i.e. if they got the last attempt from the previous day correct, it would pull the same list + the step-up amount. If incorrect, it will just pull the same list and allow them the full number of attempts to complete that list.)

**Second Week Task:**
- Participants will pull 3 words from the unused list on the first day.
  - If they get this correct, it will step up 5 still, but pull 8 new words from the unused list.
  - If they get the 8-word list incorrect 8 times, it will step down to 7 words of that 8-word list. They will get 7 attempts at the 7-word list. If they get that list correct, it will add the 8th word back and allow them 8 attempts. If they get that list correct, it will step up 3 and pick 11 new words from the list.
- Words also pull from the previous day. If they got the previous list correct, it will be a new set of words, if incorrect, it would show them that list from the previous day.

## How Version Control Works
The version control is performed on the editor's local repository. You will create a new version tag after a commit. The version tag naming convention is as follows:

- V#1.#2.#3
  - #1 = The major release number. i.e. first release would be 1, second release is 2, etc.
  - #2 = What draft are you on? i.e. second draft would be 2
  - #3 = Have the builds been updated to where development is? If the builds have not been updated, this is 0. If they have, this is 1.
  - Example: The second draft of the first major release with the builds updated would be **v1.2.1**

## How to download
You can either zip the files from the desired tag (version), or you can clone from a forked copy of the repository.
## How to edit
Please fork a copy of the repo if you are going to make any changes. Then merge any changes with the original version via a pull request.
