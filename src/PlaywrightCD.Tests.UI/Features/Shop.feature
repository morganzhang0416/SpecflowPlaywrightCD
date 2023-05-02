Feature: Shop

@regression
Scenario Outline:  user can search item
	Given I navigate to the website home page
	When I tpype keyword <item> in search box
	Then Eeach reuslt should  include keyword <item>

	Examples:
		| item  |
		| milk  |
		| bread |

