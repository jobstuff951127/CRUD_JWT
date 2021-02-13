<template >
	<v-app background-color="light-blue">
		<v-container class="fill-height" fluid background-color="light-blue">
			<v-row align="center" justify="center">
				<v-col cols="12" sm="5" md="4">
					<v-card color="">
						<v-card-text>
							<v-container fluid color="">
								<v-row>
									<v-col cols="12" class="text-center">
										<h3 class="headline primary--text">TOKA</h3>
										<h4 class="subtitle-1 mb-3">Hello</h4>
									</v-col>
								</v-row>
								<v-row align="center">
									<v-col cols="12" class="align-center">
										<v-form ref="form" v-model="valid" lazy-validation>
											<v-text-field
												v-model="form.email"
												label="Email"
												type="email"
												outlined
												dense
												:rules="[rules.required, rules.emailRules]"
												required
											/>
											<v-text-field
												id="password"
												v-model="form.password"
												label="Password"
												outlined
												dense
												:rules="[rules.required]"
												required
												:append-icon="
													visibility ? 'visibility_off' : 'visibility'
												"
												:type="visibility ? 'text' : 'password'"
												@click:append="() => (visibility = !visibility)"
											/>
											<div class="d-flex mt-5">
												<v-btn
													text
													color="primary"
													class="text-none px-2 __btn-login-text"
													disabled
												>
													Create account
												</v-btn>
												<v-spacer />
												<v-btn
													color="primary"
													class="text-none px-2"
													:disabled="!valid"
													@click="login()"
													:loading="requestStatus"
												>
													Login
												</v-btn>
											</div>
										</v-form>
									</v-col>
								</v-row>
							</v-container>
						</v-card-text>
					</v-card>
				</v-col>
			</v-row>
			<template v-slot:activator="{ on }">
				<div v-on="on">
					<v-switch
						class="mt-6"
						v-model="$vuetify.theme.dark"
						v-on="on"
						color="white"
						@click="ChangeColors"
					></v-switch>
				</div>
			</template>
		</v-container>
	</v-app>
</template>

<script>
	import agent from "@/api/agent";

	export default {
		data: () => ({
			visibility: false,
			secs: 2000,
			requestStatus: false,
			valid: true,
			form: {
				email: "",
				password: "",
			},
			users: [],
			rules: {
				required: (value) => !!value || "Required.",
				counterName: (value) =>
					!value || value.length <= 30 || "Max 30 characters",
				counterDesc: (value) =>
					!value || value.length <= 55 || "Max 55 characters",
				NoSpecial: (value) => !value || value.length <= 55 || "Max 55 characters",
				emailRules: (v) => /.+@.+/.test(v) || "E-mail must be valid",
			},
		}),

		created() {},

		methods: {
			login() {
				var isValid = this.$refs.form.validate();
        this.requestStatus = true;
				setTimeout(() => {
					if (isValid)
						agent.Users.logIn(this.form)
							.then((response) => {
								localStorage.setItem(
									"user",
									JSON.stringify(response.data.userName)
								);
								localStorage.setItem("jwt", response.data.token);
								if (localStorage.getItem("jwt") != null) {
									this.$router.push("/");
								}
							})
							.catch(() => console.log())
							.finally(() => (this.requestStatus = false));
				}, this.secs);
			},
		},
	};
</script>
<style scoped>
	.v-application {
		background-color: #edebeb;
	}
</style>