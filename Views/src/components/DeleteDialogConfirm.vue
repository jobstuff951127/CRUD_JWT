<template>
	<v-dialog v-model="visible" max-width="280">
		<v-card>
			<v-card-title class="headline">Please confirm</v-card-title>
			<v-card-text>
				You're about to delete this costumer:
				<span class="font-weight-bold">{{ Name }}</span>
			</v-card-text>
			<v-card-actions>
				<v-btn color="red" @click="visible = !visible" text>Cancel</v-btn>
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
</template>

<script>
	import agent from "@/api/agent";

	export default {
		name: "DeleteDialogConfirm",
		props: ["Name"],

		data: () => ({
			loaderConditionSave: false,
			visible: true,
		}),

		computed: {},

		watch: {},

		created() {},

		methods: {
			async updateCondition() {
				this.loaderConditionSave = true;
				this.visible = false;

				setTimeout(() => {
					agent.Costumers.delete(this.form.id)
						.then((response) => {
							if (response.status === 200) {
								this.loaderConditionSave = false;
								this.visible = true;
								this.success = "Costumer deleted successfully!";
							}
							this.ToList();
						})
						.finally(() => {
							this.visible = false;
							this.loaderConditionSave = false;
							this.close();
						});
				}, this.secs);
			},
		},
	};
</script>