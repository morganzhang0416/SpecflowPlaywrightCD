Feature: Shop

@regression
Scenario Outline:  user can search item
	Given I navigate to the website home page
	When I type keyword <item> in search box
	Then Eeach reuslt should  include keyword <item>

	Examples:
		| item  |
		| milk  |
		| bread |

@regression
Scenario Outline:  user add item to cart without sign in
    Given I navigate to the website home page
    When I type keyword <item> in search box
    And I click Add to trolley on the <nth> search result
    Then I should see sign in alert

    Examples:
        | item | nth |
        | milk | 2   |

@regression
Scenario Outline:  user add item to cart with sign in
    Given I navigate to the website sign in page
	When I login with users credentials
		| UserName        | Password     |
		| morgan.zhang0416@gmail.com | Gogo_123. |
    Then the user should be logged in successfully
    When I type keyword <item> in search box
    And I click Add to trolley on the <nth> search result
    And I click trolley button
    Then I should see item added
    When I click clearTrolleyButton
    Then I should see no items in trolley

    Examples:
        | item | nth |
        | milk | 2   |

Scenario: Sort the product search results
    Given I have searched for "milk"
    When I sort the results by "PriceAsc"
    Then I should see the milk sorted from "low-price-to-high-price"