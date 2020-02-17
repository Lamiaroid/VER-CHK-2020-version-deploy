# VER-CHK
# ***** REQUIREMENTS *****
MongoDB v4.2.3 or higher;
Visual Studio;
Internet Connection;
# ************************

Create (if doesn't exist) folder C:/data/db;
Start MondoDB Server;
Open project in Visual Studio and Start;

# ***** REMARKS *****
If you want to enable restore password by email function, you should change the following lines in ArticleService.cs to your login and password and enable "Less secure apps to access Gmail" option in your google account.
    ```c#
    // take your login
    const string SenderLogin = "test@gmai.com";
    ```

    ```c#
    // take your password
    const string SenderPassword = "admin";
    ```
# ********************

# ***** HOW TO USE *****
You can: 
-create new articles, edit and delete them (if you have already logged in);
-create new profiles;
-search articles by filters (particle title occurrency, author name and category);
-view any article simply by clicking on it in the articles list;
-post comments (if you have already logged in);
-receive forgotten passwords by email (if you have already set sender password and login);

You cannot:
-create accounts with the same names or emails;
-create accounts with empty names, emails or passwords;
-post empty comments or comments larger than 200 symbols;
-create articles with empty titles, content or categories;
-create articles with the same titles or content larger than 2000 symbols;
-change articles data on empty content or categories;
-receive password if your email is incorrect or empty;
# **********************
