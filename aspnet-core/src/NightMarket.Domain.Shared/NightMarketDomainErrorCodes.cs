﻿namespace NightMarket;

public static class NightMarketDomainErrorCodes
{
	/* You can add your business exception error codes here, as constants */

	public const string ProductNameAlreadyExists = "NightMarket:00001";
	public const string ProductCodeAlreadyExists = "NightMarket:00002";
	public const string ProductSKUAlreadyExists = "NightMarket:00003";
	public const string ProductIsNotExists = "NightMarket:00004";
	public const string ProductAttributeIsNotExists = "NightMarket:00005";

	public const string ProductAttributeValueIsNotValid = "NightMarket:00006";

	public const string RoleNameAlreadyExists = "NightMarket:00007";
}
