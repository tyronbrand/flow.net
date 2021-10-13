transaction(publicKey: String) {
	prepare(signer: AuthAccount) {
		signer.addPublicKey(publicKey.decodeHex())
	}
}
