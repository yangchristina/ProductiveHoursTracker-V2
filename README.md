# Productive hours tracker

Converted [yangchristina/productive-hours-tracker](https://github.com/yangchristina/productive-hours-tracker) from Java to C#, changed it back to a console application, and extended to use MySQL instead of JSON for data storage, and allow exporting data to an Excel spreadsheet.

## Track your energy, focus, and motivational levels throughout the day

*What will the application do?*

The application will allow the user to log their energy, focus and motivation levels every hour,
and will display their peak energy periods and the time when the user's energy levels are at their lowest. This method 
of logging energy, focus and motivation levels as a way to calculate peak productivity hours was created by Chris 
Bailey, however Chris Bailey used a spreadsheet instead of an application. I hope to make the process of tracking energy
levels easier through creating an application that helps a person do so.

*Who will use it?*

People who want to figure out their 'Biological Prime Time,' as coined by Sam Carpenter, in order to better plan out 
when to do which type of tasks. For example, a person might want to know when their peak productivity periods are
so that they spend this period doing more analytical tasks. Conversely, they may want to put routine tasks, 
such as checking emails into a time when energy levels are the lowest.

*Why is this project of interest to you?*

My interest for this project stemmed from the book *When: The Scientific Secrets of Perfect Timing* by Daniel H. Pink.
In this book, the author talks about the benefits of tracking energy levels to increase one's productivity and efficiency. 
I made this application in order to track my energy levels so that I can use my time more efficiently by plan my day according 
to my productivity levels.

## User Stories
- As a user, I want to be able to log energy, focus and motivation levels out of 10 for a certain time of day
- As a user, I want to be able to quickly log my productivity levels for my current time of day
- As a user, I want to be able to view my past logs, ordered by date of log
- As a user, I want to be able to edit and delete my past productivity entry logs
- As a user, I want to be able to view my peak and trough productivity times
- As a user, I want to be able to log in and log out of my account 
- As a user, I want to be able to save all my user information, which includes all my productivity logs, to file 
- As a user, I want my user data to be loaded from file when I log in

### Sources for methods of tracking energy levels
- https://alifeofproductivity.com/calculate-biological-prime-time/
- https://www.makeuseof.com/tag/discover-productive-hours-simple-method/
