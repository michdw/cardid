# cardid
A self-study flashcard site

The concept for this site is based on a group project I participated in for the Tech Elevator coding boot camp, in which we were assigned to create a self-study flashcard site according to a list of specifications (user stories).

After completing the boot camp, I decided to create my own version from the ground up, adding some additional features and
retooling others. My goal was to give myself the experience of putting together a full, functional website that could be useful
to anyone who wants to use online flashcards.

This site was developed and deployed using:
- C# / .NET Framework
- Visual Studio 2017
- SQL Server / SSMS
- Dapper ORM
- JQuery
- AppHarbor

DESIGN
- Page background is randomly selected from a collection of different color variations upon initial loading and upon loading certain pages within the site. Backgrounds were all created by me.
- Custom fonts are used under license:
  - Text from Google Fonts
  - Icons from Icomoon
- When certain key actions are attempted (adding a tag, deleting a deck, changing a password etc.) a temporary message will appear at the top of the newly reloaded page, indicating whether the action has been completed.

LOGIN / REGISTER
- A user account is necessary to use the features of this site. Most pages can only be accessed after logging in.
- If the user clicks on one of the links in the header (Decks, Search) before logging in, they are redirected to the Login View; after login, they are then directed to their previously selected link.
- The Login View contains a link for first-time users to go to the Register View instead.
- All actions in the code requiring a login include a method to check for a user ID. If none is found, the site redirects to an error page. This prevents users from manually typing in a url they are not authorized to access.
- When a user first logs in or registers, their homepage displays a customized welcome banner with options to create a new deck or find a deck to study.
- When a new user registers, the program checks to make sure their display name and email are not already associated with a different user.
- After login, the user homepage displays three leaderboard lists, with current data gleaned from the databases. The last two lists contain links to the appropriate items:
  - Most active users, according to number of study sessions completed
  - Most active decks, by the number of times users have studied them (public decks only)
  - Most used tags, by the number of decks asssociated with them.
- Users can expand the leaderboards from 10 to 20 items, and collapse them back to the original size.

DECKS
- The Main Deck View loads all decks by default. Users can narrow the scope by searching for all or part of a deck’s name, or selecting from a list of tags.
- Decks created by the user appear at the top, followed by public decks created by others.
- Each deck is displayed with the number of cards it contains, links to begin a study session,  and (if it a shared public deck) the name of the user who has shared it.
- The user can click on each deck to view the cards and (if it their own) edit the deck.
- In the Edit Deck View, users can:
  - Change their deck between public (visible to all registered users) and private (visible only to self)
  - Rename the deck
  - Change tags (add, remove or create). All tags created are public and available for others to use.
  - Delete the deck (after a final confirmation)
  - Add or remove cards
  - Edit the contents of each card by going to the Edit Card View (by clicking a button on the card, or by double clicking on the card text fields)
- Tags are sorted alphabetically by default, but can also be sorted by popularity (with a toggle switch).

SEARCH
- The Search Text View returns results for any specified text contained in cards, decks, or tags.
  - Initially, a search returns the number of matches found in each category with an option to expand and show all in that category.
  - When one category is selected, the other two will automatically be hidden.

ACCOUNT
- The user’s Account View contains an option to change their display name, email, and password, or delete their account (after a final confirmation).
  - As with registering a new user, the controller actions in the code ensure that all names and emails are unique among users.
- Users can view tags that they have created. If no other user has adopted their tag, they can delete it, which will remove it from any decks.
- Users can view a history of their study sessions, containing the deck studied, the date and time of the session, and the final score.
  - Sessions are sorted by time (most recent first). The top five are displayed by default, but the list can be expanded to show all.

STUDY SESSIONS
- Cards in a deck are shuffled (the order randomized) before each study session.
- The user chooses to view text on the front of each card and guess the back, or vice versa.
- Cards appear on the screen one at a time.
  - At first, a button for flipping the card appears just above.
  - Once the card is flipped, two buttons appear, for marking the score right or wrong.
  - The session’s progress is displayed onscreen:
    - Number of cards remaining in the deck
    - Number of correct guesses so far
    - Number of total guesses so far
- At the end of the session:
  - The score and percentage is displayed.
  - Any cards marked incorrect have been temporarily stored in the program’s data. The user has the option to review these cards.
  - The user can also study the entire deck again, viewing either front or back first.
      
DATABASES
- Databases (in SQL Server) are used to store information about:
  - Users (display name, email, password)
  - Decks (deck name, creator, whether it is public or private)
  - Cards (front and back text)
  - Tags (tag name, creator)
  - Study sessions (user, deck, total score, possible score, date and time)
  - Associations between decks and cards (one to many) and decks and tags (many to many)

