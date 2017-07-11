Feature: Fines
   As a librarian, 
   I want to assess fines for books returned late,
   So that we can encourage maximum book availability.
   
Scenario: Due date for book is 21 days after checkout
   Given a library system with one book
   When a patron checks out the book on 2017/3/1
   Then the due date is 2017/3/22