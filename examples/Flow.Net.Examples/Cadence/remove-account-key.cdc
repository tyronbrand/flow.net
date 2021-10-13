transaction(keyIndex: Int) {
	prepare(signer: AuthAccount) {
		signer.removePublicKey(keyIndex)
	}
}
