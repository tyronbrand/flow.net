# Emulator Examples

This project contains code samples that interact with the [Flow Emulator](https://github.com/onflow/flow/blob/master/docs/content/emulator/index.md).

- [Running the examples](#getting-started)
    - [Create Account](#create-account)
    - [Deploy/Update/Delete Contract](#deploy-contract)
    - [Query Events](#query-events)
    - [Execute Script](#execute-script)
    - [Transaction Signing](#transaction-signing)
        - [Single Party, Single Signature](#single-party-single-signature)
        - [Single Party, Multiple Signatures](#single-party-multiple-signatures)
        - [Multiple Parties](#multiple-parties)
        - [Multiple Parties, Two authorizers](#multiple-parties-two-authorizers)
        - [Multiple Parties, Multiple Signatures](#multiple-parties-multiple-signatures)

## Getting started

1. Follow [these steps](https://docs.onflow.org/flow-cli/install/) to install the Flow CLI.
2. Clone the [project](https://github.com/tyronbrand/flow.net).
3. Open terminal and navigate to the directory containing the examples project eg. (\flow.net\examples\Flow.Net.Examples).
    <img src="./emulator-start.png" alt="terminal" height="auto">
4. Start the emulator by running the following command _in this directory_:	
    ```sh
    flow emulator start -v
    ```    
5. Open Solution and uncomment the examples you wish to run.

### Create Account

[Create a new account on Flow.](./CreateAccountExample.cs)

### Deploy Contract

[Deploy/Update/Delete a Cadence smart contract.](./DeployUpdateDeleteContractExample.cs)

### Query Events

[Query events emitted by transactions.](./GetEventsForHeightRangeExample.cs)

### Execute Script

[Execute script.](./ExecuteScriptAtLatestBlockExample.cs)

### Transaction Signing

#### Single Party, Single Signature

[Sign a transaction with a single account.](./SinglePartySingleSignatureExample.cs)

#### Single Party, Multiple Signatures

[Sign a transaction with a single account using multiple signatures.](./SinglePartyMultiSignatureExample.cs)

#### Multiple Parties

[Sign a transaction with multiple accounts.](./MultiPartySingleSignatureExample.cs)

#### Multiple Parties, Two authorizers

[Sign a transaction with multiple accounts and authorize for both of them.](./MultiPartyTwoAuthorizersExample.cs)

#### Multiple Parties, Multiple Signatures

[Sign a transaction with multiple accounts using multiple signatures.](./MultiPartyMultiSignatureExample.cs)