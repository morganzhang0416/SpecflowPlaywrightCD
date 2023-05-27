Feature: Login & registration

@regression
Scenario Outline: Active user can log in successfully
	Given I navigate to the website sign in page
	When I login with a valid users credentials <userName> <password>
	Then the user should be logged in successfully

	Examples:
		| userName                | password     |
		|morgan.zhang0416@gmail.com| Gogo_123.|

@regression
Scenario: Locked Out user cannot log in successfully
	Given I navigate to the website sign in page
	When I login with users credentials
		| UserName        | Password     |
		| simon.zhang416@gmail.com | Gogo_123. |
	Then the user should not be logged in

Scenario: Successful registration
    Given I am on the registration page
    When I enter eamil and password details
    And I click on the add you details button
    Then I should see add your details page
    When I enter valid details on add your details page
	And I click on submit button
	Then I should see welcome message

