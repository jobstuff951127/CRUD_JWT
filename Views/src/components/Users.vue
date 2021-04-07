<template>
	<v-layout align-start>
		<v-flex>
			<v-data-table
				:headers="headers"
				:items="costumers"
				:search="search"
				:loading="loader"
				hide-default-footer
				@page-count="pageCount = $event"
				:page.sync="page"
				item-class="red--text"
				loading-text="Loading Costumers..."
				class="elevation-3"
			>
				<template v-slot:top>
					<v-toolbar flat color>
						<div v-if="$vuetify.breakpoint.smAndDown">
							<v-icon large color="primary">recent_actors</v-icon>
						</div>
						<div v-else>
							<v-toolbar-title>Costumers</v-toolbar-title>
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
									class="ml-3"
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
									<span class="headline primary--text">{{ formTitle }}</span>
									<v-spacer></v-spacer>
									<v-icon color="primary" large left>person_add</v-icon>
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
															:rules="[rules.required, rules.maxChars]"
															maxlength="100"
															prepend-icon="badge"
															counter
															required
														></v-text-field>
													</v-col>
													<v-col cols="12" sm="6" md="12">
														<v-text-field
															v-model="form.LastName"
															label="Last Name"
															:rules="[rules.required, rules.maxChars]"
															maxlength="100"
															prepend-icon="badge"
															counter
															required
														></v-text-field>
													</v-col>
													<v-col cols="12" sm="6" md="12">
														<v-text-field
															v-model="form.Cellphone"
															label="Cellphone"
															:rules="[rules.required, rules.minNumbers]"
															maxlength="10"
															prepend-icon="add_to_home_screen"
															type="number"
															counter
															required
														></v-text-field>
													</v-col>
													<v-col cols="12" sm="6" md="12">
														<v-menu
															ref="menu"
															v-model="menu"
															:close-on-content-click="false"
															transition="scale-transition"
															offset-y
															min-width="auto"
														>
															<template v-slot:activator="{ on, attrs }">
																<v-text-field
																	v-model="form.BirthDate"
																	label="Birthday date"
																	prepend-icon="today"
																	readonly
																	v-bind="attrs"
																	v-on="on"
																	:rules="[rules.required]"
																	required
																></v-text-field>
															</template>
															<v-date-picker
																v-model="date"
																ref="picker"
																color="primary"
																@contextmenu:year="contextMenu"
																@click:date="clickOnDate"
																@mouseenter:month="mouseEnter"
																@mouseleave:month="mouseLeave"
																locale="es"
																elevation="25"
																:max="new Date().toISOString().substr(0, 10)"
																min="1920-01-01"
															>
															</v-date-picker>
														</v-menu>
													</v-col>
													<v-col cols="12" sm="6" md="12">
														<v-text-field
															v-model="form.Adress"
															label="Adress"
															maxlength="100"
															counter
															prepend-icon="night_shelter"
															:rules="[rules.required, rules.maxChars]"
															required
														></v-text-field>
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
										color="primary"
										text
										@click="save"
										:loading="loaderSave"
										:disabled="!valid"
										>Save</v-btn
									>
								</v-card-actions>
							</v-card>
						</v-dialog>
						<!-- <DeleteDialogConfirm v-if="adModal" v-bind:Name="form.FirstName" /> -->

						<v-dialog v-model="adModal" max-width="280">
							<v-card>
								<v-card-title class="headline">Please confirm</v-card-title>
								<v-card-text>
									You're about to delete this costumer:
									<span class="font-weight-bold">{{ form.FirstName }}</span>
								</v-card-text>
								<v-card-actions>
									<v-btn color="red" @click="adModal = !adModal" text
										>Cancel</v-btn
									>
									<v-spacer></v-spacer>
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
				<template v-slot:item.actions="{ item }">
					<v-icon dense color="primary" class="mr-2" @click="editItem(item)">
						create
					</v-icon>
					<v-icon dense color="primary" @click="confirmLogicalDelete(item)">
						delete
					</v-icon>
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
			done: [false, false, false],
			mouseMonth: null,
			date: new Date().toISOString().substr(0, 10),
			form: {
				id: 0,
				FirstName: "",
				SecondName: "",
				Cellphone: "",
				Adress: "",
				BirthDate: "",
				Status: true,
			},
			page: 1,
			pageCount: 0,
			itemsPerPage: 10,
			valid: false,
			adModal: false,
			menu: false,
			success: "",
			secs: 2000,
			costumers: [],
			dialog: false,
			loader: true,
			loaderSave: false,
			loaderConditionSave: false,
			toastSave: false,
			rules: {
				required: (value) => !!value || "Required.",
				maxChars: (value) =>
					!value || value.length <= 100 || "Max 100 characters",
				minNumbers: (value) => !value || value.length === 10 || "Min 10 numbers",
			},
			headers: [
				{ text: "FirstName", value: "firstName" },
				{ text: "LastName", value: "lastName" },
				{ text: "Cellphone", value: "cellphone" },
				{ text: "Adress", value: "adress" },
				{ text: "BirthDate", value: "birthDate" },
				{ text: "Actions", value: "actions", sortable: false },
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
			menu(val) {
				val && setTimeout(() => (this.$refs.picker.activePicker = "YEAR"));
			},
		},
		created() {
			setTimeout(() => {
				this.ToList();
			}, this.secs);
		},
		methods: {
			contextMenu(event) {
				this.$set(this.done, 2, true);
				event.preventDefault();
			},
			clickOnDate(date) {
				this.$set(this.done, 0, true);
				this.form.BirthDate = date;
				this.menu = false;
			},
			mouseEnter(month) {
				this.$set(this.done, 1, true);
				this.mouseMonth = month;
			},
			mouseLeave() {
				this.mouseMonth = null;
			},
			async ToList() {
				await agent.Costumers.list()
					.then((response) => {
						this.costumers = [];
						this.costumers = response.data;
					})
					.catch(() => console.log())
					.finally(() => (this.loader = false));
			},
			async editItem(item) {
				this.form.id = item.id;
				this.form.FirstName = item.firstName;
				this.form.LastName = item.lastName;
				this.form.Cellphone = item.cellphone;
				this.form.Adress = item.adress;
				this.form.BirthDate = item.birthDate;
				this.form.Status = true;

				this.editedIndex = 1;
				this.dialog = true;
			},
			confirmLogicalDelete(item) {
				this.adModal = 1;
				this.form.id = item.id;
				this.form.FirstName = item.firstName;
				this.form.LastName = item.lastName;
				this.form.Cellphone = item.cellphone;
				this.form.Adress = item.adress;
				this.form.BirthDate = item.birthDate;
			},
			async updateCondition() {
				this.loaderConditionSave = true;
				this.toastSave = false;

				setTimeout(() => {
					agent.Costumers.delete(this.form.id)
						.then((response) => {
							if (response.status === 200) {
								this.loaderConditionSave = false;
								this.toastSave = true;
								this.success = "Costumer deleted successfully!";
							}
							this.ToList();
						})
						.finally(() => {
							this.adModal = false;
							this.loaderConditionSave = false;
							this.close();
						});
				}, this.secs);
			},
			close() {
				this.$refs.form.reset();
				this.dialog = false;
				this.clean();
			},
			clean() {
				this.form.id = 0;
				this.form.FirstName = "";
				this.form.LastName = "";
				this.form.BirthDate = "";
				this.form.Cellphone = "";
				this.date = new Date().toISOString().substr(0, 10);
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
						agent.Costumers.update(this.form)
							.then((response) => {
								if (response.status === 200) {
									this.loaderSave = false;
									this.toastSave = true;
									this.success = "Costumer edited successfully!";
								}
								this.ToList();
								this.clean();
							})
							.finally(() => {
								this.close();
								this.loaderSave = false;
							});
					} else {
						agent.Costumers.create(this.form)
							.then((response) => {
								if (response.status === 200) {
									this.ToList();
									this.clean();
									this.toastSave = true;
									this.loaderSave = false;
									this.success = "Costumer created successfully!";
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