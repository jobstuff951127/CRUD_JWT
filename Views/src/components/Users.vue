<template>
	<v-layout align-start>
		<v-flex>
			<v-data-table
				:headers="headers"
				:items="users"
				:search="search"
				:loading="loader"
				hide-default-footer
				@page-count="pageCount = $event"
				:page.sync="page"
				item-class="red--text"
				loading-text="Loading users..."
				class="elevation-1"
			>
				<template v-slot:top>
					<v-toolbar flat color>
						<div v-if="$vuetify.breakpoint.smAndDown">
							<v-icon large color="primary">manage_accounts</v-icon>
						</div>
						<div v-else>
							<v-toolbar-title>Users</v-toolbar-title>
						</div>
						<v-divider class="mx-4" inset vertical></v-divider>
						<v-spacer></v-spacer>
						<v-text-field
							class="text-xs-center"
							v-model="search"
							append-icon="search"
							label="Search"
							single-line
							hide-details
						></v-text-field>
						<v-spacer></v-spacer>
						<v-tooltip left color="primary">
							<template v-slot:activator="{ on, attrs }">
								<v-icon
									large
									v-bind="attrs"
									class="mx-6"
									color="primary"
									v-on="on"
									@click="dialog = true"
									>add_circle_outline</v-icon
								>
							</template>
							<span>Add</span>
						</v-tooltip>

						<v-dialog v-model="dialog" max-width="500px">
							<v-card>
								<v-card-title>
									<span class="headline">{{ formTitle }}</span>
								</v-card-title>
								<v-card-text>
									<v-container>
										<v-row>
											<v-flex>
												<v-form ref="form" v-model="valid" lazy-validation>
													<v-col cols="12" sm="6" md="12">
														<v-text-field
															v-model="form.FirstName"
															label="First Name"
															:rules="[rules.required, rules.counterName]"
															maxlength="30"
															counter
															required
														></v-text-field>
													</v-col>
													<v-col cols="12" sm="6" md="12">
														<v-text-field
															v-model="form.LastName"
															label="Last Name"
															:rules="[rules.required, rules.counterDesc]"
															maxlength="55"
															counter
															required
														></v-text-field>
													</v-col>
													<v-col cols="12" sm="6" md="12">
														<v-text-field
															v-model="form.Cellphone"
															label="Cellphone"
															:rules="[rules.required, rules.counterDesc]"
															maxlength="55"
															counter
															required
														></v-text-field>
													</v-col>
													<v-col cols="12" sm="6" md="12">
														<v-text-field
															v-model="form.Adress"
															label="Adress"
															:rules="[rules.required, rules.counterDesc]"
															maxlength="55"
															counter
															required
														></v-text-field>
													</v-col>

													<v-col cols="12" sm="6" md="12">
														<v-menu
															v-model="menu2"
															:close-on-content-click="false"
															:nudge-right="40"
															transition="scale-transition"
															offset-y
															min-width="auto"
														>
															<template v-slot:activator="{ on, attrs }">
																<v-text-field
																	v-model="form.BirthDate"
																	label="BirthDate"
																	prepend-icon="today"
																	readonly
																	v-bind="attrs"
																	v-on="on"
																></v-text-field>
															</template>
															<v-date-picker
																v-model="form.BirthDate"
																@input="menu2 = false"
															></v-date-picker>
														</v-menu>
													</v-col>
												</v-form>
											</v-flex>
										</v-row>
									</v-container>
								</v-card-text>

								<v-card-actions>
									<v-spacer></v-spacer>
									<v-btn color="red" text @click="close">Cancel</v-btn>
									<v-btn
										color="blue darken-1"
										text
										@click="save"
										:loading="loaderSave"
										:disabled="!valid"
										>Save</v-btn
									>
								</v-card-actions>
							</v-card>
						</v-dialog>
						<v-dialog v-model="adModal" max-width="280">
							<v-card>
								<v-card-title class="headline" v-if="!form.condition"
									>Activate item?</v-card-title
								>
								<v-card-title class="headline" v-else
									>Deactivate Item?</v-card-title
								>

								<v-card-text>
									You're about to
									<span class="font-weight-bold" v-if="!form.condition"
										>activate</span
									>
									<span class="font-weight-bold" v-else>deactivate</span>
									this item:
									<span class="font-weight-bold">{{ form.name }}</span>
								</v-card-text>
								<v-card-actions>
									<v-spacer></v-spacer>
									<v-btn color="red" @click="adModal = !adModal" text
										>Cancel</v-btn
									>
									<v-btn
										color="primary"
										@click="updateCondition"
										:loading="loaderConditionSave"
										text
										>Accept</v-btn
									>
								</v-card-actions>
							</v-card>
						</v-dialog>
					</v-toolbar>
				</template>
			</v-data-table>
			<div class="text-center pt-2 align-bottom">
				<v-pagination
					color="primary"
					v-model="page"
					:length="pageCount"
				></v-pagination>
			</div>
			<SuccessSnackBar v-if="toastSave" v-bind:success="success" />
		</v-flex>
	</v-layout>
</template>

<script>
	import SuccessSnackBar from "@/components/SuccessSnackBar";
	import agent from "@/api/agent";

	export default {
		components: {
			SuccessSnackBar,
		},
		data: () => ({
			form: {
				id: 0,
				FirstName: "",
				SecondName: "",
				Cellphone: "",
				Adress: "",
				BirthDate: "",
			},
			page: 1,
			pageCount: 0,
			itemsPerPage: 10,
			valid: false,
			adModal: false,
			menu2: false,
			success: "",
			secs: 3000,
			users: [],
			dialog: false,
			loader: true,
			loaderSave: false,
			loaderConditionSave: false,
			toastSave: false,
			rules: {
				required: (value) => !!value || "Required.",
				counterName: (value) =>
					!value || value.length <= 30 || "Max 30 characters",
				counterDesc: (value) =>
					!value || value.length <= 55 || "Max 55 characters",
				NoSpecial: (value) => !value || value.length <= 55 || "Max 55 characters",
			},
			headers: [
				{ text: "FirstName", value: "firstName" },
				{ text: "LastName", value: "lastName" },
				{ text: "Cellphone", value: "cellphone" },
				{ text: "Adress", value: "adress" },
				{ text: "BirthDate", value: "birthDate" },
			],
			search: "",
			editedIndex: -1,
		}),
		computed: {
			formTitle() {
				return this.editedIndex === -1 ? "New user" : "Edit user";
			},
		},
		watch: {
			dialog(val) {
				val || this.close();
			},
		},
		created() {
			setTimeout(() => {
				this.ToList();
			}, this.secs);
		},
		methods: {
			async ToList() {
				await agent.Users.list()
					.then((response) => {
						console.log(response);
						this.users = response.data;
					})
					.catch(() => console.log())
					.finally(() => (this.loader = false));
			},
			async editItem(item) {
				this.form.id = item.idCategory;
				this.form.name = item.name;
				this.form.description = item.description;
				this.editedIndex = 1;
				this.dialog = true;
			},
			confirmLogicalDelete(item) {
				console.log(item);
				this.adModal = 1;
				this.form.name = item.name;
				this.form.id = item.idCategory;
				this.form.condition = !item.condition;
			},
			async updateCondition() {
				this.loaderConditionSave = true;
				this.toastSave = false;

				setTimeout(() => {
					agent.Users.updateCondition(this.form)
						.then((response) => {
							if (response.status === 200) {
								this.loaderConditionSave = false;
								this.toastSave = true;
								this.success = "Condition edited successfully!";
							}
							this.ToList();
						})
						.finally(() => {
							this.adModal = false;
							this.loaderConditionSave = false;
							this.clean();
						});
				}, this.secs);
			},
			close() {
				this.$refs.form.reset();
				this.dialog = false;
			},
			clean() {
				this.form.id = 0;
				this.form.firstName = "";
				this.form.LastName = "";
				this.form.BirthDate = "";
				this.form.Cellphone = "";
				this.editedIndex = -1;
			},
			async save() {
				if (!this.validate()) {
					return;
				}
				this.loaderSave = true;
				this.toastSave = false;
				setTimeout(() => {
					if (this.editedIndex > -1) {
						agent.Users.update(this.form)
							.then((response) => {
								if (response.status === 200) {
									this.loaderSave = false;
									this.toastSave = true;
									this.success = "Category edited successfully!";
								}
								this.ToList();
								this.clean();
							})
							.finally(() => {
								this.close();
								this.loaderSave = false;
							});
					} else {
						agent.Categories.create(this.form)
							.then((response) => {
								if (response.status === 200) {
									this.ToList();
									this.clean();
									this.toastSave = true;
									this.loaderSave = false;
									this.success = "Category created successfully!";
								}
							})
							.finally(() => {
								this.close();
								this.loaderSave = false;
							});
					}
				}, this.secs);
			},
			validate() {
				this.$refs.form.validate();
				return this.$refs.form.validate();
			},
		},
	};
</script>