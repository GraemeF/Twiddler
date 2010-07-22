Feature: Authorization
	In order to interact with Twitter
	As a Twitter user
	I want to authorize Twidder with Twitter
	
Scenario: Not authorized with Twitter
	Given I have not previously authorized
	When I start the application
	Then the authorization status should show I am unauthorized
